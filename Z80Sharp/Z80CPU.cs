using System;
using Z80Sharp.Instructions;

namespace Z80Sharp
{
    public class Z80CPU : IZ80CPU
    {
        public Z80RegisterFile Registers { get; }
        public Z80CPULines ControlLines { get; }
        
        private long _cycleCount;

        public Z80CPU(Z80CPULines lines)
        {
            Registers = new Z80RegisterFile();
            ControlLines = lines;

            lines.AttachCpu(this);
        }

        public void Tick()
        {
            var instruction = InstructionDecoder.DecodeNextInstruction(this, out var instrBytes);
            _cycleCount += instruction.Execute(this, instrBytes);

            if (_cycleCount != ControlLines.SystemClock.Ticks)
            {
                throw new InvalidOperationException($"Cycle mismatch when executing instruction {instruction.Mnemonic}");
            }
        }

        private void PostMachineCycle()
        {

        }

        private void PreMachineCycle()
        {

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
