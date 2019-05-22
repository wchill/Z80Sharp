using System;
using System.Linq;

namespace Z80Sharp.Instructions
{
    public class DisassembledInstruction : IInstruction
    {
        public byte[] Opcode => _instruction.Opcode;
        public string Mnemonic { get; }
        public bool Undocumented => _instruction.Undocumented;
        public int InstructionLength => _instruction.InstructionLength;
        public bool ControlInstruction => _instruction.ControlInstruction;
        public byte[] InstructionBytes { get; }
        public ushort Address { get; }

        private IInstruction _instruction { get; }

        public DisassembledInstruction(IInstruction instruction, byte[] instrBytes, ushort address)
        {
            _instruction = instruction;
            InstructionBytes = instrBytes;
            Address = address;
            Mnemonic = GetMnemonic(_instruction.Mnemonic, instrBytes);
        }

        public int Execute(IZ80CPU cpu, byte[] instruction)
        {
            return _instruction.Execute(cpu, instruction);
        }

        private static string GetMnemonic(string mnemonic, byte[] bytes)
        {
            if (mnemonic == null) return null;

            if (mnemonic.Contains("nn"))
            {
                var operand = Utilities.LETo16Bit(bytes[bytes.Length - 2], bytes[bytes.Length - 1]);
                mnemonic = mnemonic.Replace("nn", operand.ToString("X") + "h");
            }

            if (mnemonic.Contains("n"))
            {
                mnemonic = mnemonic.Replace("n", bytes.Last().ToString("X") + "h");
            }

            if (mnemonic.Contains("d"))
            {
                mnemonic = mnemonic.Replace("d", bytes[2].ToString("X") + "h");
            }

            return mnemonic;
        }

        public override string ToString()
        {
            return Undocumented ? $"[{Address:X4}h] {Mnemonic} (undocumented)" : $"[{Address:X4}h] {Mnemonic}";
        }
    }
}