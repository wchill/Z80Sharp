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

        private readonly Func<IZ80CPU, byte[], int> _action;

        public Instruction(byte opcode, string mnemonic, Func<IZ80CPU, byte[], int> action)
        {
            Opcode = new[] { opcode };
            Mnemonic = mnemonic;
            Undocumented = false;
            _action = action;
        }

        public Instruction(byte[] opcode, string mnemonic, Func<IZ80CPU, byte[], int> action)
            : this(opcode, mnemonic, false, action)
        {
        }

        public Instruction(byte[] opcode, string mnemonic, bool undocumented, Func<IZ80CPU, byte[], int> action)
        {
            Opcode = opcode;
            Mnemonic = mnemonic;
            Undocumented = undocumented;
            _action = action;
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