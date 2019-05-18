using Z80Sharp.Instructions;

namespace Z80Sharp
{
    public class Z80RegisterFile
    {
        public ushort AF { get; set; }
        public ushort BC { get; set; }
        public ushort DE { get; set; }
        public ushort HL { get; set; }
        public ushort SP { get; set; }
        public ushort PC { get; set; }
        public ushort IX { get; set; }
        public ushort IY { get; set; }
        public byte I { get; set; }
        public byte R { get; set; }
        public ushort AF_Shadow { get; set; }
        public ushort BC_Shadow { get; set; }
        public ushort DE_Shadow { get; set; }
        public ushort HL_Shadow { get; set; }

        // IFF1 disables maskable interrupts
        public bool IFF1 { get; set; }

        // Saves the value of IFF1 during a nonmaskable interrupt
        public bool IFF2 { get; set; }

        public Z80InterruptMode InterruptMode { get; set; }

        public byte A
        {
            get => Utilities.GetUpperByteOfWord(AF);
            set => AF = Utilities.SetUpperByteOfWord(AF, value);
        }

        public byte F
        {
            get => Utilities.GetLowerByteOfWord(AF);
            set => AF = Utilities.SetLowerByteOfWord(AF, value);
        }

        public byte B
        {
            get => Utilities.GetUpperByteOfWord(BC);
            set => BC = Utilities.SetUpperByteOfWord(BC, value);
        }

        public byte C
        {
            get => Utilities.GetLowerByteOfWord(BC);
            set => BC = Utilities.SetLowerByteOfWord(BC, value);
        }

        public byte D
        {
            get => Utilities.GetUpperByteOfWord(DE);
            set => DE = Utilities.SetUpperByteOfWord(DE, value);
        }

        public byte E
        {
            get => Utilities.GetLowerByteOfWord(DE);
            set => DE = Utilities.SetLowerByteOfWord(DE, value);
        }

        public byte H
        {
            get => Utilities.GetUpperByteOfWord(HL);
            set => HL = Utilities.SetUpperByteOfWord(HL, value);
        }

        public byte L
        {
            get => Utilities.GetLowerByteOfWord(HL);
            set => HL = Utilities.SetLowerByteOfWord(HL, value);
        }

        public byte IXLower
        {
            get => Utilities.GetLowerByteOfWord(IX);
            set => IX = Utilities.SetLowerByteOfWord(IX, value);
        }

        public byte IXUpper
        {
            get => Utilities.GetUpperByteOfWord(IX);
            set => IX = Utilities.SetUpperByteOfWord(IX, value);
        }

        public byte IYLower
        {
            get => Utilities.GetLowerByteOfWord(IY);
            set => IY = Utilities.SetLowerByteOfWord(IY, value);
        }

        public byte IYUpper
        {
            get => Utilities.GetUpperByteOfWord(IY);
            set => IY = Utilities.SetUpperByteOfWord(IY, value);
        }

        public bool Sign
        {
            get => Utilities.GetBit(F, 7);
            set => F = Utilities.SetBit(F, value, 7);
        }

        public bool Zero
        {
            get => Utilities.GetBit(F, 6);
            set => F = Utilities.SetBit(F, value, 6);
        }

        public bool F5
        {
            get => Utilities.GetBit(F, 5);
            set => F = Utilities.SetBit(F, value, 5);
        }

        public bool HalfCarry
        {
            get => Utilities.GetBit(F, 4);
            set => F = Utilities.SetBit(F, value, 4);
        }

        public bool F3
        {
            get => Utilities.GetBit(F, 3);
            set => F = Utilities.SetBit(F, value, 3);
        }

        public bool ParityOrOverflow
        {
            get => Utilities.GetBit(F, 2);
            set => F = Utilities.SetBit(F, value, 2);
        }

        public bool Subtract
        {
            get => Utilities.GetBit(F, 1);
            set => F = Utilities.SetBit(F, value, 1);
        }

        public bool Carry
        {
            get => Utilities.GetBit(F, 0);
            set => F = Utilities.SetBit(F, value, 0);
        }

        public void SwapAFWithShadow()
        {
            var temp = AF;
            AF = AF_Shadow;
            AF_Shadow = temp;
        }

        public void SwapAddressRegistersWithShadow()
        {
            var temp = BC;
            BC = BC_Shadow;
            BC_Shadow = temp;
            temp = DE;
            DE = DE_Shadow;
            DE_Shadow = temp;
            temp = HL;
            HL = HL_Shadow;
            HL_Shadow = temp;
        }
    }
}