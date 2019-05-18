using Xunit;
using Z80Sharp;

namespace Z80SharpTests
{
    public class UtilitiesTests
    {
        [Theory]
        [InlineData(0xFF, 0, 8, 0xFF)]
        [InlineData(0xFF, 0, 1, 0x01)]
        [InlineData(0xFF, 0, 3, 0x07)]
        [InlineData(0xFF, 1, 3, 0x0E)]
        public void TestExtractBits(int num, int start, int len, int expected)
        {
            Assert.Equal(expected, Utilities.ExtractBits(num, start, len));
        }

        [Theory]
        [InlineData(0xFF, 7, 0x80)]
        [InlineData(0x80, 7, 0x80)]
        [InlineData(0x12, 1, 0x02)]
        [InlineData(0x00, 7, 0x00)]
        public void TestGetBitAsByte(int num, int bitNum, int expected)
        {
            Assert.Equal(expected, Utilities.GetBitAsByte(num, bitNum));
            Assert.Equal(expected != 0, Utilities.GetBit(num, bitNum));
        }

        [Theory]
        [InlineData(0xFF, true, 7, 0xFF)]
        [InlineData(0x12, true, 3, 0x1A)]
        [InlineData(0x00, true, 7, 0x80)]
        [InlineData(0xFF, false, 7, 0x7F)]
        [InlineData(0x12, false, 1, 0x10)]
        [InlineData(0x00, false, 7, 0x00)]
        public void TestSetBit(byte num, bool bit, int bitNum, byte expected)
        {
            Assert.Equal(expected, Utilities.SetBit(num, bit, bitNum));
        }
    }
}