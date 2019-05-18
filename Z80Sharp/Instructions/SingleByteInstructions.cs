using System;
using System.Linq;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static class SingleByteInstructions
    {
        public static readonly IInstruction[] Instructions = ConstructInstructionsByReflection();

        private static IInstruction[] ConstructInstructionsByReflection()
        {
            var instructions = new IInstruction[256];
            var methods = typeof(Z80Instructions).GetMethods();
            foreach (var method in methods)
            {
                var attrs = method.GetAttributes<MainInstructionAttribute>();
                if (!attrs.Any()) continue;
                foreach (var attr in attrs)
                {
                    var func = (Func<IZ80CPU, byte[], int>)Delegate.CreateDelegate(typeof(Func<IZ80CPU, byte[], int>), method);
                    instructions[attr.Opcode[0]] = new SingleByteInstruction(attr.Opcode[0], attr.Mnemonic, func);
                }
            }

            var text = $"Loaded {instructions.Count(instr => instr != null)} instructions.";
            Console.WriteLine(text);

            for (var i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == null)
                {
                    instructions[i] = new SingleByteInstruction((byte) i, null, InstructionDecoder.UnimplementedOpcode);
                }
            }

            return instructions;
        }
    }
}