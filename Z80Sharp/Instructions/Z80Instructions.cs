using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        #region Helper methods
        private static void WriteByteToCpuRegister(IZ80CPU cpu, int register, byte data)
        {
            switch (register)
            {
                case 0b111: cpu.Registers.A = data; break;
                case 0b000: cpu.Registers.B = data; break;
                case 0b001: cpu.Registers.C = data; break;
                case 0b010: cpu.Registers.D = data; break;
                case 0b011: cpu.Registers.E = data; break;
                case 0b100: cpu.Registers.H = data; break;
                case 0b101: cpu.Registers.L = data; break;
                default: throw new InvalidOperationException();
            }
        }

        private static void WriteWordToCpuRegister_BC_DE_HL_SP(IZ80CPU cpu, int register, ushort data)
        {
            switch (register)
            {
                case 0b00: cpu.Registers.BC = data; break;
                case 0b01: cpu.Registers.DE = data; break;
                case 0b10: cpu.Registers.HL = data; break;
                case 0b11: cpu.Registers.SP = data; break;
                default: throw new InvalidOperationException();
            }
        }

        private static void WriteWordToCpuRegister_BC_DE_HL_AF(IZ80CPU cpu, int register, ushort data)
        {
            switch (register)
            {
                case 0b00: cpu.Registers.BC = data; break;
                case 0b01: cpu.Registers.DE = data; break;
                case 0b10: cpu.Registers.HL = data; break;
                case 0b11: cpu.Registers.AF = data; break;
                default: throw new InvalidOperationException();
            }
        }

        private static byte ReadByteFromCpuRegister(IZ80CPU cpu, int register)
        {
            switch (register)
            {
                case 0b111: return cpu.Registers.A;
                case 0b000: return cpu.Registers.B;
                case 0b001: return cpu.Registers.C;
                case 0b010: return cpu.Registers.D;
                case 0b011: return cpu.Registers.E;
                case 0b100: return cpu.Registers.H;
                case 0b101: return cpu.Registers.L;
                default: throw new InvalidOperationException();
            }
        }

        private static ushort ReadWordFromCpuRegister_BC_DE_HL_SP(IZ80CPU cpu, int register)
        {
            switch (register)
            {
                case 0b00: return cpu.Registers.BC;
                case 0b01: return cpu.Registers.DE;
                case 0b10: return cpu.Registers.HL;
                case 0b11: return cpu.Registers.SP;
                default: throw new InvalidOperationException();
            }
        }

        private static ushort ReadWordFromCpuRegister_BC_DE_HL_AF(IZ80CPU cpu, int register)
        {
            switch (register)
            {
                case 0b00: return cpu.Registers.BC;
                case 0b01: return cpu.Registers.DE;
                case 0b10: return cpu.Registers.HL;
                case 0b11: return cpu.Registers.AF;
                default: throw new InvalidOperationException();
            }
        }
        #endregion
    }
}
