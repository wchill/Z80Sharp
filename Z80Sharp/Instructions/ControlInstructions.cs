using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        #region General

        [MainInstruction("NOP", 1, 0x00)]
        public static int NOP(IZ80CPU cpu, byte[] instruction)
        {
            return 4;
        }

        [MainInstruction("HALT", 1, 0x76)]
        public static int HALT(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.HALT.WriteValue(cpu, TristateWireState.LogicLow);
            cpu.Registers.PC--;
            return 4;
        }

        [MainInstruction("DI", 1, 0xF3)]
        public static int DI(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.IFF1 = false;
            cpu.Registers.IFF2 = false;

            return 4;
        }

        [MainInstruction("EI", 1, 0xFB)]
        public static int EI(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.IFF1 = true;
            cpu.Registers.IFF2 = true;

            return 4;
        }

        [ExtendedInstruction("IM 0", 2, 0xED, 0x46)]
        public static int IM_0(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.InterruptMode = Z80InterruptMode.External;

            return 8;
        }

        [ExtendedInstruction("IM 1", 2, 0xED, 0x56)]
        public static int IM_1(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.InterruptMode = Z80InterruptMode.FixedAddress;

            return 8;
        }

        [ExtendedInstruction("IM 2", 2, 0xED, 0x5E)]
        public static int IM_2(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.InterruptMode = Z80InterruptMode.Vectorized;

            return 8;
        }

        #endregion

        #region Jump

        [MainInstruction("JP nn", 3, 0xC3)]
        public static int JP_nn(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.PC = Utilities.LETo16Bit(instruction[1], instruction[2]);
            return 10;
        }

        [MainInstruction("JP NZ, nn", 3, 0xC2)]
        [MainInstruction("JP Z, nn", 3, 0xCA)]
        [MainInstruction("JP NC, nn", 3, 0xD2)]
        [MainInstruction("JP C, nn", 3, 0xDA)]
        [MainInstruction("JP PO, nn", 3, 0xE2)]
        [MainInstruction("JP PE, nn", 3, 0xEA)]
        [MainInstruction("JP P, nn", 3, 0xF2)]
        [MainInstruction("JP M, nn", 3, 0xFA)]
        public static int JP_cc_nn(IZ80CPU cpu, byte[] instruction)
        {
            var condition = instruction[0].ExtractBits(3, 3);
            var shouldJump = false;

            switch (condition)
            {
                case 0b000: shouldJump = !cpu.Registers.Zero; break;
                case 0b001: shouldJump = cpu.Registers.Zero; break;
                case 0b010: shouldJump = !cpu.Registers.Carry; break;
                case 0b011: shouldJump = cpu.Registers.Carry; break;
                case 0b100: shouldJump = !cpu.Registers.ParityOrOverflow; break;
                case 0b101: shouldJump = cpu.Registers.ParityOrOverflow; break;
                case 0b110: shouldJump = !cpu.Registers.Sign; break;
                case 0b111: shouldJump = cpu.Registers.Sign; break;
                default: throw new InvalidOperationException();
            }

            if (shouldJump)
            {
                cpu.Registers.PC = Utilities.LETo16Bit(instruction[1], instruction[2]);
            }
            return 10;
        }

        [MainInstruction("JR e", 2, 0x18)]
        public static int JR_e(IZ80CPU cpu, byte[] instruction)
        {
            var offset = (int) instruction[1];
            offset += 2;

            if (offset < 0)
            {
                cpu.Registers.PC -= (ushort) Math.Abs(offset);
            }
            else
            {
                cpu.Registers.PC += (ushort) offset;
            }
            
            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 12;
        }

        [MainInstruction("JR C, e", 2, 0x38)]
        [MainInstruction("JR NC, e", 2, 0x30)]
        [MainInstruction("JR Z, e", 2, 0x28)]
        [MainInstruction("JR NZ, e", 2, 0x20)]
        public static int JR_cc_e(IZ80CPU cpu, byte[] instruction)
        {
            var condition = instruction[0].ExtractBits(3, 2);
            var shouldJump = false;

            switch (condition)
            {
                case 0b00: shouldJump = !cpu.Registers.Zero; break;
                case 0b01: shouldJump = cpu.Registers.Zero; break;
                case 0b10: shouldJump = !cpu.Registers.Carry; break;
                case 0b11: shouldJump = cpu.Registers.Carry; break;
                default: throw new InvalidOperationException();
            }

            if (!shouldJump) return 7;

            var offset = (int)instruction[1];
            offset += 2;

            if (offset < 0)
            {
                cpu.Registers.PC -= (ushort)Math.Abs(offset);
            }
            else
            {
                cpu.Registers.PC += (ushort)offset;
            }

            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 12;

        }

        [MainInstruction("JP (HL)", 1, 0xE9)]
        public static int JP_HL(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.PC = cpu.Registers.HL;
            return 4;
        }

        [IXInstruction("JP (IX)", 2, 0xDD, 0xE9)]
        public static int JP_IX(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.PC = cpu.Registers.IX;
            return 8;
        }

        [IYInstruction("JP (IY)", 2, 0xFD, 0xE9)]
        public static int JP_IY(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.PC = cpu.Registers.IY;
            return 8;
        }

        [MainInstruction("DJNZ, e", 2, 0x10)]
        public static int DJNZ_e(IZ80CPU cpu, byte[] instruction)
        {
            // FIXME: The opcode fetch has the tick after it, then the memory read for the operand
            cpu.ControlLines.SystemClock.Tick();

            cpu.Registers.B--;

            if (cpu.Registers.B == 0) return 8;

            var offset = (int)instruction[1];
            offset += 2;

            if (offset < 0)
            {
                cpu.Registers.PC -= (ushort)Math.Abs(offset);
            }
            else
            {
                cpu.Registers.PC += (ushort)offset;
            }

            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 13;
        }

        #endregion
    }
}
