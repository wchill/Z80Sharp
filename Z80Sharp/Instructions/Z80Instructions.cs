using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        [MainInstruction("NOP", 1, 0x00)]
        public static int NOP(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            return 1;
        }

        [MainInstruction("INC BC", 1, 0x03)]
        [MainInstruction("INC DE", 1, 0x13)]
        [MainInstruction("INC HL", 1, 0x23)]
        [MainInstruction("INC SP", 1, 0x33)]
        public static int INC_ss(IZ80CPU cpu, byte[] instruction)
        {
            switch (instruction[0])
            {
                case 0x03: cpu.Registers.BC++; break;
                case 0x13: cpu.Registers.DE++; break;
                case 0x23: cpu.Registers.HL++; break;
                case 0x33: cpu.Registers.SP++; break;
                default: throw new InvalidOperationException();
            }
            cpu.ControlLines.SystemClock.TickMultiple(2);

            return 6;
        }

        [MainInstruction("INC A", 1, 0x3C)]
        [MainInstruction("INC B", 1, 0x04)]
        [MainInstruction("INC C", 1, 0x0C)]
        [MainInstruction("INC D", 1, 0x14)]
        [MainInstruction("INC E", 1, 0x1C)]
        [MainInstruction("INC H", 1, 0x24)]
        [MainInstruction("INC L", 1, 0x2C)]
        public static int INC_r(IZ80CPU cpu, byte[] instruction)
        {
            byte val;
            switch (instruction[0])
            {
                case 0x3C: cpu.Registers.A++; val = cpu.Registers.A; break;
                case 0x04: cpu.Registers.B++; val = cpu.Registers.B; break;
                case 0x0C: cpu.Registers.C++; val = cpu.Registers.C; break;
                case 0x14: cpu.Registers.D++; val = cpu.Registers.D; break;
                case 0x1C: cpu.Registers.E++; val = cpu.Registers.E; break;
                case 0x24: cpu.Registers.H++; val = cpu.Registers.H; break;
                case 0x2C: cpu.Registers.L++; val = cpu.Registers.L; break;
                default: throw new InvalidOperationException();
            }

            cpu.Registers.Sign = val > 127;
            cpu.Registers.Zero = val == 0;
            cpu.Registers.HalfCarry = (val & 0b1111) == 0;
            cpu.Registers.ParityOrOverflow = val == 0x80;
            cpu.Registers.Subtract = false;

            return 4;
        }

        [MainInstruction("DEC A", 1, 0x3D)]
        [MainInstruction("DEC B", 1, 0x05)]
        [MainInstruction("DEC C", 1, 0x0D)]
        [MainInstruction("DEC D", 1, 0x15)]
        [MainInstruction("DEC E", 1, 0x1D)]
        [MainInstruction("DEC H", 1, 0x25)]
        [MainInstruction("DEC L", 1, 0x2D)]
        public static int DEC_r(IZ80CPU cpu, byte[] instruction)
        {
            byte val;
            switch (instruction[0])
            {
                case 0x3C: cpu.Registers.A--; val = cpu.Registers.A; break;
                case 0x04: cpu.Registers.B--; val = cpu.Registers.B; break;
                case 0x0C: cpu.Registers.C--; val = cpu.Registers.C; break;
                case 0x14: cpu.Registers.D--; val = cpu.Registers.D; break;
                case 0x1C: cpu.Registers.E--; val = cpu.Registers.E; break;
                case 0x24: cpu.Registers.H--; val = cpu.Registers.H; break;
                case 0x2C: cpu.Registers.L--; val = cpu.Registers.L; break;
                default: throw new InvalidOperationException();
            }

            cpu.Registers.Sign = Utilities.IsUnsignedByteNegative(val);
            cpu.Registers.Zero = val == 0;
            cpu.Registers.HalfCarry = (val & 0b1111) == 0b1111;
            cpu.Registers.ParityOrOverflow = val == 0x7f;
            cpu.Registers.Subtract = true;

            return 4;
        }

        [MainInstruction("RLCA", 1, 0x07)]
        public static int RLCA(IZ80CPU cpu, byte[] instruction)
        {
            var temp = (cpu.Registers.A << 1) | (Utilities.GetBitAsByte(cpu.Registers.A, 7) >> 7);
            cpu.Registers.A = (byte)(temp & 0xFF);
            cpu.Registers.Carry = Utilities.GetBit(cpu.Registers.A, 0);
            cpu.Registers.HalfCarry = false;
            cpu.Registers.Subtract = false;

            return 4;
        }

        [MainInstruction("RLA", 1, 0x17)]
        public static int RLA(IZ80CPU cpu, byte[] instruction)
        {
            var carry = Utilities.GetBit(cpu.Registers.A, 7);
            var temp = (cpu.Registers.A << 1) | (cpu.Registers.Carry ? 1 : 0);
            cpu.Registers.A = (byte)(temp & 0xFF);
            cpu.Registers.Carry = carry;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.Subtract = false;

            return 4;
        }
    }
}
