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
        public static byte GetUpperByte(this ushort word)
        {
            return (byte)((word & 0xFF00) >> 8);
        }

        public static byte GetLowerByte(this ushort word)
        {
            return (byte)(word & 0xFF);
        }

        public static ushort SetUpperByte(this ushort word, byte upper)
        {
            return (ushort)(word.GetLowerByte() | (upper << 8));
        }

        public static ushort SetLowerByte(this ushort word, byte lower)
        {
            return (ushort)((word.GetUpperByte() << 8) | lower);
        }

        public static byte SetBit(this byte input, bool bit, int bitNum)
        {
            var mask = 1 << bitNum;
            var ret = input & ~mask;
            ret |= (bit ? 1 : 0) << bitNum;
            return (byte)ret;
        }

        public static bool GetBit(this int input, int bitNum)
        {
            return (input & (1 << bitNum)) != 0;
        }
        public static bool GetBit(this ushort input, int bitNum)
        {
            return (input & (1 << bitNum)) != 0;
        }
        public static bool GetBit(this byte input, int bitNum)
        {
            return (input & (1 << bitNum)) != 0;
        }

        public static byte GetBitAsByte(this int input, int bitNum)
        {
            return (byte)(input & (1 << bitNum));
        }

        public static byte GetBitAsByte(this ushort input, int bitNum)
        {
            return (byte)(input & (1 << bitNum));
        }

        public static byte GetBitAsByte(this byte input, int bitNum)
        {
            return (byte)(input & (1 << bitNum));
        }

        public static byte GetLowerNibble(this byte input)
        {
            return (byte) (input & 0xF);
        }

        public static byte GetUpperNibble(this byte input)
        {
            return (byte)((input & 0xF0) >> 4);
        }

        public static byte SetLowerNibble(this byte input, byte nibble)
        {
            return (byte)((input & 0xF0) | (nibble & 0xF));
        }

        public static byte SetUpperNibble(this byte input, byte nibble)
        {
            return (byte)((input & 0xF) | (nibble & 0xF) << 4);
        }

        public static int ExtractBits(this int input, int lowerBitNum, int numBits)
        {
            var mask = ((1 << numBits) - 1) << lowerBitNum;
            return (input >> lowerBitNum) & mask;
        }

        public static ushort ExtractBits(this ushort input, int lowerBitNum, int numBits)
        {
            return (ushort)((int)input).ExtractBits(lowerBitNum, numBits);
        }

        public static byte ExtractBits(this byte input, int lowerBitNum, int numBits)
        {
            return (byte)((int)input).ExtractBits(lowerBitNum, numBits);
        }

        public static bool IsNegative(this ushort b)
        {
            return GetBit(b, 15);
        }

        public static bool IsNegative(this byte b)
        {
            return GetBit(b, 7);
        }

        public static bool WillHalfCarry(this byte a, byte b)
        {
            return (((a & 0xf) + (b & 0xf)) & 0x10) == 0x10;
        }

        public static bool WillCarry(this byte a, byte b)
        {
            return GetBit(a + b, 8);
        }

        public static bool WillOverflow(this byte a, byte b)
        {
            var sum = a + b;
            var flag = ~(a ^ b) & (a ^ sum) & 0x80;
            return GetBit(flag, 7);
        }

        public static int GetNumBitsSet(this byte a)
        {
            // TODO: .NET Core 3.0 has the PopCnt intrinsic. Use that when available.
            var i = a - ((a >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        public static bool IsParityEven(this byte a)
        {
            return (GetNumBitsSet(a) & 0x1) == 0;
        }

        public static byte TwosComplement(this byte a)
        {
            return (byte)(~a + 1);
        }

        public static ushort TwosComplement(this ushort a)
        {
            return (ushort)(~a + 1);
        }

        public static ushort CalculateIndex(this ushort index, byte offset)
        {
            return (ushort)(index + (sbyte)offset);
        }
    }
}
