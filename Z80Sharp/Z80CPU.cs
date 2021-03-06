﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z80Sharp.Instructions;

namespace Z80Sharp
{
    public class Z80CPU : IZ80CPU
    {
        public Z80RegisterFile Registers { get; }
        public Z80CPULines ControlLines { get; }
        public Stack<ushort> InterruptsBeingServiced { get; }

        private long _cycleCount;

        public Z80CPU(Z80CPULines lines)
        {
            Registers = new Z80RegisterFile();
            ControlLines = lines;
            InterruptsBeingServiced = new Stack<ushort>();

            ControlLines.AttachCpu(this);
        }

        public void Tick()
        {
            if (InterruptsBeingServiced.Any())
            {
                _cycleCount++;
            }

            if (ControlLines.RESET.Value == TristateWireState.LogicLow)
            {
                ControlLines.SystemClock.DetachClockableDevice(this);
                HandleRESET();
                ControlLines.SystemClock.AttachClockableDevice(this);
            }
        }

        public void ExecuteNextInstruction()
        {
            if (Registers.PC == 0)
            {
                throw new InvalidOperationException("Hit address 0");
            }
            if (Registers.PC == 5)
            {
                // Handle CP/M syscalls
                switch (Registers.C)
                {
                    case 2:
                    {
                        var inputByte = Registers.E;
                        var outputChar = Encoding.ASCII.GetString(new[] {inputByte})[0];
                        Console.Write(outputChar);
                        break;
                    }
                    case 9:
                    {
                        var strAddress = Registers.DE;
                        var inputBytes = new List<byte>();
                        byte inputByte;
                        _cycleCount += 3;
                        while ((inputByte = ReadMemory(strAddress)) != '$')
                        {
                            _cycleCount += 3;
                            inputBytes.Add(inputByte);
                            strAddress++;
                        }
                        var outputChar = Encoding.ASCII.GetString(inputBytes.ToArray());
                        Console.Write(outputChar);
                        break;
                    }
                    default:
                        // throw new NotImplementedException($"Unimplemented syscall {Registers.C}");
                        break;
                }

                FetchOpcode();
                _cycleCount += Z80Instructions.RET(this, null);
            }

            var instruction = InstructionDecoder.DecodeNextInstruction(this);

            var beforeCount = _cycleCount;
            _cycleCount += instruction.Execute(this, instruction.InstructionBytes);

            if (_cycleCount != ControlLines.SystemClock.Ticks)
            {
                throw new InvalidOperationException($"Cycle mismatch when executing instruction {instruction.Mnemonic} - expected {_cycleCount - beforeCount} but got {ControlLines.SystemClock.Ticks - beforeCount} cycles");
            }

            if (Registers.IFF1 && ControlLines.INT.Value == TristateWireState.LogicLow)
            {
                HandleINT();
            }
        }

        private void HandleRESET()
        {
            Registers.IFF1 = false;
            Registers.IFF2 = false;
            Registers.PC = 0;
            Registers.I = 0;
            Registers.R = 0;
            Registers.InterruptMode = Z80InterruptMode.External;

            ControlLines.AddressBus.WriteValue(this, null);
            ControlLines.DataBus.WriteValue(this, null);
            ControlLines.HALT.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.M1.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicHigh);

            ControlLines.SystemClock.TickMultiple(3);

            while (ControlLines.RESET.Value == TristateWireState.LogicLow)
            {
                ControlLines.SystemClock.Tick();
            }
        }

        private void HandleNMI()
        {
            Registers.IFF2 = Registers.IFF1;
            Registers.IFF1 = false;
            AcknowledgeInterrupt();
            // TODO: Check if NMI is supposed to be able to fire during this push
            PushWord(Registers.PC);
            Registers.PC = 0x66;
        }

        private void HandleINT()
        {
            // TODO: Check if INT is supposed to be able to fire during this push and other memory operations
            var opcode = AcknowledgeInterrupt();

            if (Registers.InterruptMode == Z80InterruptMode.External)
            {
                var instr = InstructionDecoder.DecodeNextInstruction(this, opcode);
                instr.Execute(this, instr.InstructionBytes);
            }
            else if (Registers.InterruptMode == Z80InterruptMode.FixedAddress)
            {
                PushWord(Registers.PC);
                Registers.PC = 0x38;
            }
            else if (Registers.InterruptMode == Z80InterruptMode.Vectorized)
            {
                var upper = Registers.I;
                var lower = opcode;
                var addr = Utilities.LETo16Bit(lower, upper);
                PushWord(Registers.PC);
                var jumpAddr = ReadWord(addr);
                Registers.PC = jumpAddr;
            }
        }

        private byte AcknowledgeInterrupt()
        {
            InterruptsBeingServiced.Push(Registers.PC);

            // TODO: Check the timing for this
            ControlLines.HALT.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            ControlLines.AddressBus.WriteValue(this, Registers.PC);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.M1.WriteValue(this, TristateWireState.LogicLow);
            Registers.PC++;
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            ControlLines.SystemClock.TickMultiple(2);

            var opcode = ControlLines.DataBus.Value;

            RefreshMemory();

            return opcode;
        }

        private void PreMachineCycle()
        {

        }

        private void PostMachineCycle()
        {
            if (ControlLines.BUSREQ.Value == TristateWireState.LogicLow)
            {
                ControlLines.MREQ.WriteValue(this, TristateWireState.HighImpedance);
                ControlLines.IORQ.WriteValue(this, TristateWireState.HighImpedance);
                ControlLines.RD.WriteValue(this, TristateWireState.HighImpedance);
                ControlLines.WR.WriteValue(this, TristateWireState.HighImpedance);
                ControlLines.RFSH.WriteValue(this, TristateWireState.HighImpedance);
                ControlLines.BUSACK.WriteValue(this, TristateWireState.LogicLow);

                while (ControlLines.BUSREQ.Value == TristateWireState.LogicLow)
                {
                    ControlLines.SystemClock.Tick();
                }

                ControlLines.BUSACK.WriteValue(this, TristateWireState.HighImpedance);
                // TODO: Handle BUSREQ/BUSACK completely (reset wires? etc)
            }

            if (ControlLines.NMI.Value == TristateWireState.LogicLow)
            {
                HandleNMI();
            }
        }

        public byte FetchOpcode()
        {
            PreMachineCycle();

            ControlLines.AddressBus.WriteValue(this, Registers.PC);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.M1.WriteValue(this, TristateWireState.LogicLow);
            Registers.PC++;
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            var opcode = ControlLines.DataBus.Value;

            RefreshMemory();
            PostMachineCycle();

            return opcode;
        }

        public void InsertWaitMachineCycle(int numCycles)
        {
            PreMachineCycle();
            ControlLines.SystemClock.TickMultiple(numCycles);
            PostMachineCycle();
        }

        private void RefreshMemory()
        {
            Registers.R++;
            var refreshAddr = Utilities.LETo16Bit((byte)(Registers.R & 0x7F), Registers.I);

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.AddressBus.WriteValue(this, refreshAddr);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);

            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.M1.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();
        }

        public byte ReadMemory(ushort address, int waitBefore = 0, int waitAfter = 0)
        {
            PreMachineCycle();
            ControlLines.SystemClock.TickMultiple(waitBefore);

            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            ControlLines.SystemClock.TickMultiple(waitAfter);
            PostMachineCycle();

            return ControlLines.DataBus.Value;
        }

        public ushort ReadWord(ushort address)
        {
            var lower = ReadMemory(address);
            var upper = ReadMemory((ushort) (address + 1));

            return Utilities.LETo16Bit(lower, upper);
        }

        public byte PopByte()
        {
            var data = ReadMemory(Registers.SP);
            Registers.SP++;

            return data;
        }

        public void WriteMemory(ushort address, byte data, int waitBefore = 0, int waitAfter = 0)
        {
            PreMachineCycle();
            ControlLines.SystemClock.TickMultiple(waitBefore);

            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.DataBus.WriteValue(this, data);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            ControlLines.SystemClock.TickMultiple(waitAfter);
            PostMachineCycle();
        }

        public void WriteWord(ushort address, ushort data)
        {
            WriteMemory(address, data.GetLowerByte());
            address++;
            WriteMemory(address, data.GetUpperByte());
        }

        public void PushWord(ushort data, int waitBefore = 0)
        {
            Registers.SP--;
            WriteMemory(Registers.SP, data.GetUpperByte(), waitBefore);
            Registers.SP--;
            WriteMemory(Registers.SP, data.GetLowerByte());
        }

        public byte ReadFromPort(ushort address, int waitBefore = 0)
        {
            PreMachineCycle();
            ControlLines.SystemClock.TickMultiple(waitBefore);

            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            var data = ControlLines.DataBus.Value;

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();
            
            PostMachineCycle();

            return data;
        }

        public void WriteToPort(ushort address, byte data)
        {
            PreMachineCycle();

            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.DataBus.WriteValue(this, data);
            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();
            
            PostMachineCycle();
        }

        private void WaitForMemory()
        {
            // Note that this is technically incorrect, the WAIT line is sampled halfway
            // through a clock cycle. However, our emulated memory should be asserting WAIT
            // for the entire clock cycle that the actual write occurs, so it should be fine.

            // Also note that we use a do/while loop because we always have to wait at least 1
            // cycle even if the WAIT line is brought HIGH immediately.
            do
            {
                ControlLines.SystemClock.Tick();
            } while (ControlLines.WAIT.Value != TristateWireState.LogicHigh);
        }
    }
}
