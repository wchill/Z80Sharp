namespace Z80Sharp.Instructions.Attributes
{
    public class IYBitInstructionAttribute : InstructionAttribute
    {
        public IYBitInstructionAttribute(string mnemonic, int instrLength, params byte[] opcodeBytes) : base(mnemonic, instrLength, opcodeBytes)
        {
        }

        public IYBitInstructionAttribute(string mnemonic, int instrLength, bool undocumented, params byte[] opcodeBytes) : base(mnemonic, instrLength, undocumented, opcodeBytes)
        {
        }
        public override byte[] OpcodePrefix => new byte[] { 0xFD, 0xCB };
    }
}
