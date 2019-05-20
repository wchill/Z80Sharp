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
        [InlineData(0xFF, 3, 3, 0x07)]
        public void TestExtractBits(int num, int start, int len, int expected)
        {
            Assert.Equal(expected, num.ExtractBits(start, len));
        }

        [Theory]
        [InlineData(0xFF, 7, 0x80)]
        [InlineData(0x80, 7, 0x80)]
        [InlineData(0x12, 1, 0x02)]
        [InlineData(0x00, 7, 0x00)]
        public void TestGetBitAsByte(int num, int bitNum, int expected)
        {
            Assert.Equal(expected, num.GetBitAsByte(bitNum));
            Assert.Equal(expected != 0, num.GetBit(bitNum));
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
            Assert.Equal(expected, num.SetBit(bit, bitNum));
        }

        [Theory]
        [InlineData(120, 105, true)]
        [InlineData(120, -105, false)]
        [InlineData(-120, -105, true)]
        [InlineData(0, -105, false)]
        [InlineData(0, 0, false)]
        public void TestDetectOverflow(sbyte a, sbyte b, bool expected)
        {
            Assert.Equal(expected, ((byte)a).WillOverflow((byte) b));
        }

        [Theory]
        [InlineData(0b11011, 0b11100101)]
        [InlineData(0b11111111, 0b1)]
        [InlineData(0b1111111, 0b10000001)]
        public void TestTwosComplement(byte b, byte expected)
        {
            Assert.Equal(expected, b.TwosComplement());
        }

        [Theory]
        [InlineData(0x100, 0, 0x100)]
        [InlineData(0x100, 1, 0x101)]
        [InlineData(0x100, -1, 0xFF)]
        [InlineData(0x1, -2, 0xFFFF)]
        [InlineData(0xFFFF, 2, 0x1)]
        public void TestCalculateIndex(ushort index, sbyte offset, ushort expected)
        {
            Assert.Equal(expected, index.CalculateIndex((byte) offset));
        }
    }
}