using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static class InstructionDecoder
    {
        public static readonly IInstruction[] MainInstructions;
        public static readonly IInstruction[] ExtendedInstructions;
        public static readonly IInstruction[] BitInstructions;
        public static readonly IInstruction[] IXInstructions;
        public static readonly IInstruction[] IYInstructions;
        public static readonly IInstruction[] IXBitInstructions;
        public static readonly IInstruction[] IYBitInstructions;

        static InstructionDecoder()
        {
            MainInstructions = ConstructInstructionTablesByReflection<MainInstructionAttribute>();
            ExtendedInstructions = ConstructInstructionTablesByReflection<ExtendedInstructionAttribute>();
            BitInstructions = ConstructInstructionTablesByReflection<BitInstructionAttribute>();
            IXInstructions = ConstructInstructionTablesByReflection<IXInstructionAttribute>();
            IYInstructions = ConstructInstructionTablesByReflection<IYInstructionAttribute>();
            IXBitInstructions = ConstructInstructionTablesByReflection<IXBitInstructionAttribute>();
            IYBitInstructions = ConstructInstructionTablesByReflection<IYBitInstructionAttribute>();
        }

        public static int UnimplementedOpcode(IZ80CPU cpu, byte[] instruction)
        {
            throw new NotImplementedException(
                $"Instruction {string.Join(" ", instruction.Select(b => b.ToString("X2")))} is not implemented.");
        }

        public static DisassembledInstruction DecodeNextInstruction(IZ80CPU cpu, byte? initialByte = null)
        {
            var addr = cpu.Registers.PC;
            var data = new List<byte>
            {
                initialByte ?? cpu.FetchOpcode()
            };
            IInstruction instruction;

            switch (data[0])
            {
                case 0xCB:
                    data.Add(cpu.FetchOpcode());
                    instruction = BitInstructions[data[1]];
                    break;
                case 0xDD:
                    data.Add(cpu.FetchOpcode());
                    if (data[1] == 0xCB)
                    {
                        data.Add(cpu.ReadMemory(cpu.Registers.PC));
                        cpu.Registers.PC++;
                        data.Add(cpu.ReadMemory(cpu.Registers.PC));
                        cpu.Registers.PC++;
                        instruction = IXBitInstructions[data[3]];
                    }
                    else
                    {
                        instruction = IXInstructions[data[1]];
                    }
                    break;
                case 0xED:
                    data.Add(cpu.FetchOpcode());
                    instruction = ExtendedInstructions[data[1]];
                    break;
                case 0xFD:
                    data.Add(cpu.FetchOpcode());
                    if (data[1] == 0xCB)
                    {
                        data.Add(cpu.ReadMemory(cpu.Registers.PC));
                        cpu.Registers.PC++;
                        data.Add(cpu.ReadMemory(cpu.Registers.PC));
                        cpu.Registers.PC++;
                        instruction = IYBitInstructions[data[3]];
                    }
                    else
                    {
                        instruction = IYInstructions[data[1]];
                    }
                    break;
                default:
                    instruction = MainInstructions[data[0]];
                    break;
            }

            while (data.Count < instruction.InstructionLength)
            {
                data.Add(cpu.ReadMemory(cpu.Registers.PC));
                cpu.Registers.PC++;
            }
            
            return new DisassembledInstruction(instruction, data.ToArray(), addr);
        }

        private static IInstruction[] ConstructInstructionTablesByReflection<T>() where T : InstructionAttribute
        {
            var instructions = new IInstruction[256];
            var methods = typeof(Z80Instructions).GetMethods();
            foreach (var method in methods)
            {
                var attrs = method.GetAttributes<T>();
                if (!attrs.Any()) continue;

                var isControl = method.GetCustomAttributes(typeof(ControlInstructionAttribute), true).Any();

                foreach (var attr in attrs)
                {
                    var func = (Func<IZ80CPU, byte[], int>)Delegate.CreateDelegate(typeof(Func<IZ80CPU, byte[], int>), method);
                    var instruction = new Instruction(attr.Opcode, attr.Mnemonic, attr.Length, attr.Undocumented, isControl, func);

                    if (instructions[attr.Opcode.Last()] != null)
                    {
                        throw new InvalidOperationException($"Duplicate instruction {instruction} loaded but {instructions[attr.Opcode.Last()]} exists");
                    }

                    instructions[attr.Opcode.Last()] = instruction;
                }
            }

            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == null)
                {
                    instructions[i] = new Instruction(new [] {(byte) i}, null, 1, true, true, UnimplementedOpcode);
                }
            }

            return instructions;
        }
    }
}
