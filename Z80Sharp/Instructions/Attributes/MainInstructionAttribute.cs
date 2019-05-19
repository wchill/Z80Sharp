namespace Z80Sharp.Instructions.Attributes
{
    public class MainInstructionAttribute : InstructionAttribute
    {
        public MainInstructionAttribute(string mnemonic, int instrLength, byte opcodeByte) : base(mnemonic, instrLength, opcodeByte)
        {
        }
        public override byte[] OpcodePrefix => new byte[] {};
    }
}
