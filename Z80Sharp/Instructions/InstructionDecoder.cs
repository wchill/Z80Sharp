using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static class InstructionDecoder
    {
        private static readonly IInstruction[] MainInstructions;
        private static readonly IInstruction[] ExtendedInstructions;
        private static readonly IInstruction[] BitInstructions;
        private static readonly IInstruction[] IXInstructions;
        private static readonly IInstruction[] IYInstructions;
        private static readonly IInstruction[] IXBitInstructions;
        private static readonly IInstruction[] IYBitInstructions;

        public static readonly HashSet<byte[]> LoadedOpcodes;

        static InstructionDecoder()
        {
            LoadedOpcodes = new HashSet<byte[]>();
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
                $"Instruction {string.Join(" ", instruction.Select(b => b.ToString("X4")))} is not implemented.");
        }

        public static IInstruction DecodeNextInstruction(IZ80CPU cpu, out byte[] instrBytes)
        {
            var data = new List<byte>
            {
                cpu.FetchOpcode()
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

            while (data.Count != instruction.Opcode.Length)
            {
                data.Add(cpu.ReadMemory(cpu.Registers.PC));
                cpu.Registers.PC++;
            }

            instrBytes = data.ToArray();
            return instruction;
        }

        private static IInstruction[] ConstructInstructionTablesByReflection<T>() where T : InstructionAttribute
        {
            var instructions = new IInstruction[256];
            var methods = typeof(Z80Instructions).GetMethods();
            foreach (var method in methods)
            {
                var attrs = method.GetAttributes<T>();
                if (!attrs.Any()) continue;
                foreach (var attr in attrs)
                {
                    var func = (Func<IZ80CPU, byte[], int>)Delegate.CreateDelegate(typeof(Func<IZ80CPU, byte[], int>), method);
                    var instruction = new Instruction(attr.Opcode, attr.Mnemonic, func);

                    if (!LoadedOpcodes.Add(attr.Opcode))
                    {
                        throw new InvalidOperationException($"Duplicate instruction loaded: {instruction}");
                    }

                    instructions[attr.Opcode.Last()] = instruction;
                }
            }

            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == null)
                {
                    instructions[i] = new Instruction((byte)i, null, UnimplementedOpcode);
                }
            }

            return instructions;
        }
    }
}
