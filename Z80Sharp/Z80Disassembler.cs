using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions;

namespace Z80Sharp
{
    public class Z80Disassembler : IZ80CPU
    {
        public Z80RegisterFile Registers { get; }
        public Z80CPULines ControlLines { get; }
        public Stack<ushort> InterruptsBeingServiced { get; }

        public byte[] Memory { get; }
        public Z80Disassembler(byte[] memory)
        {
            Memory = memory;
            Registers = new Z80RegisterFile();
            ControlLines = new Z80CPULines();
            InterruptsBeingServiced = new Stack<ushort>();
        }

        public List<DisassembledInstruction> Disassemble(ushort address)
        {
            Registers.PC = address;
            var instructions = new List<DisassembledInstruction>();
            DisassembledInstruction instruction;

            do
            {
                instruction = InstructionDecoder.DecodeNextInstruction(this);
                instructions.Add(instruction);
            } while (!instruction.ControlInstruction);

            return instructions;
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }

        public byte FetchOpcode()
        {
            return ReadMemory(Registers.PC++);
        }

        public void InsertWaitMachineCycle(int numCycles)
        {
            throw new NotImplementedException();
        }

        public byte ReadMemory(ushort address, int waitBefore = 0, int waitAfter = 0)
        {
            return Memory[address];
        }

        public ushort ReadWord(ushort address)
        {
            throw new NotImplementedException();
        }

        public byte PopByte()
        {
            throw new NotImplementedException();
        }

        public void WriteMemory(ushort address, byte data, int waitBefore = 0, int waitAfter = 0)
        {
            throw new NotImplementedException();
        }

        public void WriteWord(ushort address, ushort data)
        {
            throw new NotImplementedException();
        }

        public void PushWord(ushort data, int waitBefore = 0)
        {
            throw new NotImplementedException();
        }

        public byte ReadFromPort(ushort address, int waitBefore = 0)
        {
            throw new NotImplementedException();
        }

        public void WriteToPort(ushort address, byte data)
        {
            throw new NotImplementedException();
        }
    }
}
