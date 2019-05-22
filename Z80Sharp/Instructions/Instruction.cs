using System;
using System.Linq;
using System.Reflection;

namespace Z80Sharp.Instructions
{
    public class Instruction : IInstruction
    {
        public byte[] Opcode { get; }
        public string Mnemonic { get; }
        public bool Undocumented { get; }
        public int InstructionLength { get; }
        public bool ControlInstruction { get; }

        private readonly Func<IZ80CPU, byte[], int> _action;

        public Instruction(byte[] opcode, string mnemonic, int len, bool undocumented, bool controlInstruction, Func<IZ80CPU, byte[], int> action)
        {
            Opcode = opcode;
            Mnemonic = mnemonic;
            Undocumented = undocumented;
            _action = action;
            InstructionLength = len;
            ControlInstruction = controlInstruction;
        }

        public int Execute(IZ80CPU cpu, byte[] instruction)
        {
            return _action.Invoke(cpu, instruction);
        }

        public override string ToString()
        {
            var hexStr = string.Join(" ", Opcode.Select(b => b.ToString("X2")));
            if (Undocumented)
            {
                return $"{Mnemonic} (undocumented; {hexStr})";
            }
            return $"{Mnemonic} ({hexStr})";
        }
    }
}