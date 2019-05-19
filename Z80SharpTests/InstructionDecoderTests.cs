using System.Linq;
using Xunit;
using Z80Sharp.Instructions;

namespace Z80SharpTests
{
    public class InstructionDecoderTests
    {
        [Fact]
        public void TestMainInstructionList()
        {
            Assert.Equal(252, GetImplementedCount(InstructionDecoder.MainInstructions));
        }
        [Fact]
        public void TestExtendedInstructionList()
        {
            Assert.Equal(58, GetImplementedCount(InstructionDecoder.ExtendedInstructions));
        }
        [Fact]
        public void TestBitInstructionList()
        {
            Assert.Equal(248, GetImplementedCount(InstructionDecoder.BitInstructions));
        }
        [Fact]
        public void TestIXInstructionList()
        {
            Assert.Equal(39, GetImplementedCount(InstructionDecoder.IXInstructions));
        }
        [Fact]
        public void TestIYInstructionList()
        {
            Assert.Equal(39, GetImplementedCount(InstructionDecoder.IYInstructions));
        }
        [Fact]
        public void TestIXBitInstructionList()
        {
            Assert.Equal(31, GetImplementedCount(InstructionDecoder.IXBitInstructions));
        }
        [Fact]
        public void TestIYBitInstructionList()
        {
            Assert.Equal(31, GetImplementedCount(InstructionDecoder.IYBitInstructions));
        }

        private static int GetImplementedCount(IInstruction[] instructions)
        {
            return instructions.Count(instruction => instruction.Mnemonic != null);
        }
    }
}