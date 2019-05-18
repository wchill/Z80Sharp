using System;
using System.Linq;
using System.Reflection;

namespace Z80Sharp.Instructions
{
    public class SingleByteInstruction : IInstruction
    {
        public byte[] Opcode { get; }
        public string Mnemonic { get; }
        public bool IsDocumented => true;

        private readonly Func<IZ80CPU, byte[], int> _action;

        public SingleByteInstruction(byte opcode, string mnemonic, Func<IZ80CPU, byte[], int> action)
        {
            Opcode = new[] { opcode };
            Mnemonic = mnemonic;
            _action = action;
        }

        public int Execute(IZ80CPU cpu, byte[] instruction)
        {
            return _action.Invoke(cpu, instruction);
        }
    }
}