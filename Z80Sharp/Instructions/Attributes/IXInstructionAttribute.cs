namespace Z80Sharp.Instructions.Attributes
{
    public class IXInstructionAttribute : InstructionAttribute
    {
        public IXInstructionAttribute(string mnemonic, int instrLength, params byte[] opcodeBytes) : base(mnemonic, instrLength, opcodeBytes)
        {
        }

        public IXInstructionAttribute(string mnemonic, int instrLength, bool undocumented, params byte[] opcodeBytes) : base(mnemonic, instrLength, undocumented, opcodeBytes)
        {
        }
        public override byte[] OpcodePrefix => new byte[] { 0xDD };
    }
}
