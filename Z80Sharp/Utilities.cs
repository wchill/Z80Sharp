using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public static class Utilities
    {
        public static ushort LETo16Bit(byte lower, byte upper)
        {
            return (ushort) ((upper << 8) | lower);
        }

        public static byte GetUpperByteOfWord(ushort word)
        {
            return (byte)((word & 0xFF00) >> 8);
        }

        public static byte GetLowerByteOfWord(ushort word)
        {
            return (byte)(word & 0xFF);
        }

        public static ushort SetUpperByteOfWord(ushort word, byte upper)
        {
            return (ushort)(GetLowerByteOfWord(word) | (upper << 8));
        }

        public static ushort SetLowerByteOfWord(ushort word, byte lower)
        {
            return (ushort)((GetUpperByteOfWord(word) << 8) | lower);
        }

        public static byte SetBit(byte input, bool bit, int bitNum)
        {
            var mask = 1 << bitNum;
            var ret = input & ~mask;
            ret |= (bit ? 1 : 0) << bitNum;
            return (byte)ret;
        }

        public static bool GetBit(int input, int bitNum)
        {
            return (input & (1 << bitNum)) != 0;
        }

        public static byte GetBitAsByte(int input, int bitNum)
        {
            return (byte) (input & (1 << bitNum));
        }

        public static int ExtractBits(int input, int lowerBitNum, int numBits)
        {
            var mask = ((1 << numBits) - 1) << lowerBitNum;
            return (input >> lowerBitNum) & mask;
        }

        public static bool IsUnsignedByteNegative(byte b)
        {
            return (sbyte) b < 0;
        }
    }
}
