namespace Z80Sharp.Instructions.Attributes
{
    public class IXBitInstructionAttribute : InstructionAttribute
    {
        public IXBitInstructionAttribute(string mnemonic, int instrLength, params byte[] opcodeBytes) : base(mnemonic, instrLength, opcodeBytes)
        {
        }

        public IXBitInstructionAttribute(string mnemonic, int instrLength, bool undocumented, params byte[] opcodeBytes) : base(mnemonic, instrLength, undocumented, opcodeBytes)
        {
        }
        public override byte[] OpcodePrefix => new byte[] { 0xDD, 0xCB };
    }
}
