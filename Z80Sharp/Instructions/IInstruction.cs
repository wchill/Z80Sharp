using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp.Instructions
{
    public interface IInstruction
    {
        byte[] Opcode { get; }
        string Mnemonic { get; }
        bool Undocumented { get; }
        int InstructionLength { get; }
        bool ControlInstruction { get; }

        int Execute(IZ80CPU cpu, byte[] instruction);
    }
}
