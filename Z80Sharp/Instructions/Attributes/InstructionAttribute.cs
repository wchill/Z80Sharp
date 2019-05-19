using System;

namespace Z80Sharp.Instructions.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class InstructionAttribute : Attribute
    {
        public abstract byte[] OpcodePrefix { get; }
        public byte[] Opcode { get; }
        public string Mnemonic { get; }
        public int Length { get; }
        public bool Undocumented { get; }
        public InstructionAttribute(string mnemonic, int instrLength, params byte[] opcodeBytes)
        {
            Opcode = opcodeBytes;
            Mnemonic = mnemonic;
            Length = instrLength;
            Undocumented = false;
        }
        public InstructionAttribute(string mnemonic, int instrLength, bool undocumented, params byte[] opcodeBytes)
        {
            Opcode = opcodeBytes;
            Mnemonic = mnemonic;
            Length = instrLength;
            Undocumented = undocumented;
        }
    }
}
