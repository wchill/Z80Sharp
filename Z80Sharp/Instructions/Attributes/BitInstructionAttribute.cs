namespace Z80Sharp.Instructions.Attributes
{
    public class BitInstructionAttribute : InstructionAttribute
    {
        public BitInstructionAttribute(string mnemonic, int instrLength, params byte[] opcodeBytes) : base(mnemonic, instrLength, opcodeBytes)
        {
        }

        public BitInstructionAttribute(string mnemonic, int instrLength, bool undocumented, params byte[] opcodeBytes) : base(mnemonic, instrLength, undocumented, opcodeBytes)
        {
        }
    }
}
