namespace Z80Sharp.Instructions.Attributes
{
    public class IYInstructionAttribute : InstructionAttribute
    {
        public IYInstructionAttribute(string mnemonic, int instrLength, params byte[] opcodeBytes) : base(mnemonic, instrLength, opcodeBytes)
        {
        }

        public IYInstructionAttribute(string mnemonic, int instrLength, bool undocumented, params byte[] opcodeBytes) : base(mnemonic, instrLength, undocumented, opcodeBytes)
        {
        }
    }
}
