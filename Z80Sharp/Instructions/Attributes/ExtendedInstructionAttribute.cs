namespace Z80Sharp.Instructions.Attributes
{
    public class ExtendedInstructionAttribute : InstructionAttribute
    {
        public ExtendedInstructionAttribute(string mnemonic, int instrLength, params byte[] opcodeBytes) : base(mnemonic, instrLength, opcodeBytes)
        {
        }

        public ExtendedInstructionAttribute(string mnemonic, int instrLength, bool undocumented, params byte[] opcodeBytes) : base(mnemonic, instrLength, undocumented, opcodeBytes)
        {
        }
    }
}
