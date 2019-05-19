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
            get => AF.GetUpperByte();
            set => AF = AF.SetUpperByte(value);
        }

        public byte F
        {
            get => AF.GetLowerByte();
            set => AF = AF.SetLowerByte(value);
        }

        public byte B
        {
            get => BC.GetUpperByte();
            set => BC = BC.SetUpperByte(value);
        }

        public byte C
        {
            get => BC.GetLowerByte();
            set => BC = BC.SetLowerByte(value);
        }

        public byte D
        {
            get => DE.GetUpperByte();
            set => DE = DE.SetUpperByte(value);
        }

        public byte E
        {
            get => DE.GetLowerByte();
            set => DE = DE.SetLowerByte(value);
        }

        public byte H
        {
            get => HL.GetUpperByte();
            set => HL = HL.SetUpperByte(value);
        }

        public byte L
        {
            get => HL.GetLowerByte();
            set => HL = HL.SetLowerByte(value);
        }

        public byte IXUpper
        {
            get => IX.GetUpperByte();
            set => IX = IX.SetUpperByte(value);
        }

        public byte IXLower
        {
            get => IX.GetLowerByte();
            set => IX = IX.SetLowerByte(value);
        }

        public byte IYUpper
        {
            get => IY.GetUpperByte();
            set => IY = IY.SetUpperByte(value);
        }

        public byte IYLower
        {
            get => IY.GetLowerByte();
            set => IY = IY.SetLowerByte(value);
        }

        public bool Sign
        {
            get => F.GetBit(7);
            set => F = F.SetBit(value, 7);
        }

        public bool Zero
        {
            get => F.GetBit(6);
            set => F = F.SetBit(value, 6);
        }

        public bool F5
        {
            get => F.GetBit(5);
            set => F = F.SetBit(value, 5);
        }

        public bool HalfCarry
        {
            get => F.GetBit(4);
            set => F = F.SetBit(value, 4);
        }

        public bool F3
        {
            get => F.GetBit(3);
            set => F = F.SetBit(value, 3);
        }

        public bool ParityOrOverflow
        {
            get => F.GetBit(2);
            set => F = F.SetBit(value, 2);
        }

        public bool Subtract
        {
            get => F.GetBit(1);
            set => F = F.SetBit(value, 1);
        }

        public bool Carry
        {
            get => F.GetBit(0);
            set => F = F.SetBit(value, 0);
        }

        public void SwapAFWithShadow()
        {
            var temp = AF;
            AF = AF_Shadow;
            AF_Shadow = temp;
        }

        public void SwapRegisterPairsWithShadow()
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