using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        #region 8-bit Loads
        [MainInstruction("LD A, A", 1, 0x7F)]
        [MainInstruction("LD A, B", 1, 0x78)]
        [MainInstruction("LD A, C", 1, 0x79)]
        [MainInstruction("LD A, D", 1, 0x7A)]
        [MainInstruction("LD A, E", 1, 0x7B)]
        [MainInstruction("LD A, H", 1, 0x7C)]
        [MainInstruction("LD A, L", 1, 0x7D)]
        [MainInstruction("LD B, A", 1, 0x47)]
        [MainInstruction("LD B, B", 1, 0x40)]
        [MainInstruction("LD B, C", 1, 0x41)]
        [MainInstruction("LD B, D", 1, 0x42)]
        [MainInstruction("LD B, E", 1, 0x43)]
        [MainInstruction("LD B, H", 1, 0x44)]
        [MainInstruction("LD B, L", 1, 0x45)]
        [MainInstruction("LD C, A", 1, 0x4F)]
        [MainInstruction("LD C, B", 1, 0x48)]
        [MainInstruction("LD C, C", 1, 0x49)]
        [MainInstruction("LD C, D", 1, 0x4A)]
        [MainInstruction("LD C, E", 1, 0x4B)]
        [MainInstruction("LD C, H", 1, 0x4C)]
        [MainInstruction("LD C, L", 1, 0x4D)]
        [MainInstruction("LD D, A", 1, 0x57)]
        [MainInstruction("LD D, B", 1, 0x50)]
        [MainInstruction("LD D, C", 1, 0x51)]
        [MainInstruction("LD D, D", 1, 0x52)]
        [MainInstruction("LD D, E", 1, 0x53)]
        [MainInstruction("LD D, H", 1, 0x54)]
        [MainInstruction("LD D, L", 1, 0x55)]
        [MainInstruction("LD E, A", 1, 0x5F)]
        [MainInstruction("LD E, B", 1, 0x58)]
        [MainInstruction("LD E, C", 1, 0x59)]
        [MainInstruction("LD E, D", 1, 0x5A)]
        [MainInstruction("LD E, E", 1, 0x5B)]
        [MainInstruction("LD E, H", 1, 0x5C)]
        [MainInstruction("LD E, L", 1, 0x5D)]
        [MainInstruction("LD H, A", 1, 0x67)]
        [MainInstruction("LD H, B", 1, 0x60)]
        [MainInstruction("LD H, C", 1, 0x61)]
        [MainInstruction("LD H, D", 1, 0x62)]
        [MainInstruction("LD H, E", 1, 0x63)]
        [MainInstruction("LD H, H", 1, 0x64)]
        [MainInstruction("LD H, L", 1, 0x65)]
        [MainInstruction("LD L, A", 1, 0x6F)]
        [MainInstruction("LD L, B", 1, 0x68)]
        [MainInstruction("LD L, C", 1, 0x69)]
        [MainInstruction("LD L, D", 1, 0x6A)]
        [MainInstruction("LD L, E", 1, 0x6B)]
        [MainInstruction("LD L, H", 1, 0x6C)]
        [MainInstruction("LD L, L", 1, 0x6D)]
        public static int LD_r_rp(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);

            var dst = instruction[0].ExtractBits(3, 3);
            WriteByteToCpuRegister(cpu, dst, data);

            return 4;
        }

        [MainInstruction("LD A, n", 2, 0x3E)]
        [MainInstruction("LD B, n", 2, 0x06)]
        [MainInstruction("LD C, n", 2, 0x0E)]
        [MainInstruction("LD D, n", 2, 0x16)]
        [MainInstruction("LD E, n", 2, 0x1E)]
        [MainInstruction("LD H, n", 2, 0x26)]
        [MainInstruction("LD L, n", 2, 0x2E)]
        public static int LD_r_n(IZ80CPU cpu, byte[] instruction)
        {
            var dst = instruction[0].ExtractBits(3, 3);
            WriteByteToCpuRegister(cpu, dst, instruction[1]);

            return 7;
        }

        [MainInstruction("LD A, (HL)", 1, 0x7E)]
        [MainInstruction("LD B, (HL)", 1, 0x45)]
        [MainInstruction("LD C, (HL)", 1, 0x4E)]
        [MainInstruction("LD D, (HL)", 1, 0x55)]
        [MainInstruction("LD E, (HL)", 1, 0x5E)]
        [MainInstruction("LD H, (HL)", 1, 0x65)]
        [MainInstruction("LD L, (HL)", 1, 0x6E)]
        public static int LD_r_hl_mem(IZ80CPU cpu, byte[] instruction)
        {
            var dst = instruction[0].ExtractBits(3, 3);
            var data = cpu.ReadMemory(cpu.Registers.HL);
            WriteByteToCpuRegister(cpu, dst, data);

            return 7;
        }

        [IXInstruction("LD A, (IX+d)", 3, 0xDD, 0x7E)]
        [IXInstruction("LD B, (IX+d)", 3, 0xDD, 0x46)]
        [IXInstruction("LD C, (IX+d)", 3, 0xDD, 0x4E)]
        [IXInstruction("LD D, (IX+d)", 3, 0xDD, 0x56)]
        [IXInstruction("LD E, (IX+d)", 3, 0xDD, 0x5E)]
        [IXInstruction("LD H, (IX+d)", 3, 0xDD, 0x66)]
        [IXInstruction("LD L, (IX+d)", 3, 0xDD, 0x6E)]
        public static int LD_r_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var dst = instruction[0].ExtractBits(3, 3);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));

            WriteByteToCpuRegister(cpu, dst, data);
            
            return 19;
        }

        [IYInstruction("LD A, (IY+d)", 3, 0xFD, 0x7E)]
        [IYInstruction("LD B, (IY+d)", 3, 0xFD, 0x46)]
        [IYInstruction("LD C, (IY+d)", 3, 0xFD, 0x4E)]
        [IYInstruction("LD D, (IY+d)", 3, 0xFD, 0x56)]
        [IYInstruction("LD E, (IY+d)", 3, 0xFD, 0x5E)]
        [IYInstruction("LD H, (IY+d)", 3, 0xFD, 0x66)]
        [IYInstruction("LD L, (IY+d)", 3, 0xFD, 0x6E)]
        public static int LD_r_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var dst = instruction[0].ExtractBits(3, 3);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            
            WriteByteToCpuRegister(cpu, dst, data);

            return 19;
        }

        [MainInstruction("LD (HL), A", 1, 0x77)]
        [MainInstruction("LD (HL), B", 1, 0x70)]
        [MainInstruction("LD (HL), C", 1, 0x71)]
        [MainInstruction("LD (HL), D", 1, 0x72)]
        [MainInstruction("LD (HL), E", 1, 0x73)]
        [MainInstruction("LD (HL), H", 1, 0x74)]
        [MainInstruction("LD (HL), L", 1, 0x75)]
        public static int LD_HL_mem_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            cpu.WriteMemory(cpu.Registers.HL, data);

            return 7;
        }

        [IXInstruction("LD (IX+d), A", 3, 0xDD, 0x77)]
        [IXInstruction("LD (IX+d), B", 3, 0xDD, 0x70)]
        [IXInstruction("LD (IX+d), C", 3, 0xDD, 0x71)]
        [IXInstruction("LD (IX+d), D", 3, 0xDD, 0x72)]
        [IXInstruction("LD (IX+d), E", 3, 0xDD, 0x73)]
        [IXInstruction("LD (IX+d), H", 3, 0xDD, 0x74)]
        [IXInstruction("LD (IX+d), L", 3, 0xDD, 0x75)]
        public static int LD_IX_plus_d_mem_r(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var src = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            
            cpu.WriteMemory(cpu.Registers.IX.CalculateIndex(instruction[2]), data);

            return 19;
        }

        [IXInstruction("LD (IY+d), A", 3, 0xFD, 0x77)]
        [IXInstruction("LD (IY+d), B", 3, 0xFD, 0x70)]
        [IXInstruction("LD (IY+d), C", 3, 0xFD, 0x71)]
        [IXInstruction("LD (IY+d), D", 3, 0xFD, 0x72)]
        [IXInstruction("LD (IY+d), E", 3, 0xFD, 0x73)]
        [IXInstruction("LD (IY+d), H", 3, 0xFD, 0x74)]
        [IXInstruction("LD (IY+d), L", 3, 0xFD, 0x75)]
        public static int LD_IY_plus_d_mem_r(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var src = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            
            cpu.WriteMemory(cpu.Registers.IY.CalculateIndex(instruction[2]), data);

            return 19;
        }

        [MainInstruction("LD (HL), n", 2, 0x36)]
        public static int LD_HL_mem_n(IZ80CPU cpu, byte[] instruction)
        {
            cpu.WriteMemory(cpu.Registers.HL, instruction[1]);

            return 10;
        }

        [IXInstruction("LD (IX+d), n", 4, 0xDD, 0x36)]
        public static int LD_IX_plus_d_mem_n(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            cpu.WriteMemory(cpu.Registers.IX.CalculateIndex(instruction[2]), instruction[3]);

            return 19;
        }

        [IYInstruction("LD (IY+d), n", 4, 0xFD, 0x36)]
        public static int LD_IY_plus_d_mem_n(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            cpu.WriteMemory(cpu.Registers.IX.CalculateIndex(instruction[2]), instruction[3]);

            return 19;
        }

        [MainInstruction("LD A, (BC)", 1, 0x0A)]
        public static int LD_A_BC_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = cpu.ReadMemory(cpu.Registers.BC);
            return 7;
        }

        [MainInstruction("LD A, (DE)", 1, 0x1A)]
        public static int LD_A_DE_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = cpu.ReadMemory(cpu.Registers.DE);
            return 7;
        }

        [MainInstruction("LD A, (nn)", 3, 0x3A)]
        public static int LD_A_nn_mem(IZ80CPU cpu, byte[] instruction)
        {
            var addr = Utilities.LETo16Bit(instruction[1], instruction[2]);
            cpu.Registers.A = cpu.ReadMemory(addr);
            return 13;
        }

        [MainInstruction("LD (BC), A", 1, 0x02)]
        public static int LD_BC_mem_A(IZ80CPU cpu, byte[] instruction)
        {
            cpu.WriteMemory(cpu.Registers.BC, cpu.Registers.A);
            return 7;
        }

        [MainInstruction("LD (DE), A", 1, 0x12)]
        public static int LD_DE_mem_A(IZ80CPU cpu, byte[] instruction)
        {
            cpu.WriteMemory(cpu.Registers.DE, cpu.Registers.A);
            return 7;
        }

        [MainInstruction("LD (nn), A", 3, 0x32)]
        public static int LD_nn_mem_A(IZ80CPU cpu, byte[] instruction)
        {
            var addr = Utilities.LETo16Bit(instruction[1], instruction[2]);
            cpu.WriteMemory(addr, cpu.Registers.A);
            return 13;
        }

        [ExtendedInstruction("LD A, I", 1, 0xED, 0x57)]
        public static int LD_A_I(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = cpu.Registers.I;
            cpu.Registers.Sign = cpu.Registers.A.IsNegative();
            cpu.Registers.Zero = cpu.Registers.A == 0;
            cpu.Registers.HalfCarry = false;

            // TODO: Handle the case where an interrupt occurs during this instruction (parity should be 0)
            cpu.Registers.ParityOrOverflow = cpu.Registers.IFF2;
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();

            return 9;
        }

        [ExtendedInstruction("LD A, R", 1, 0xED, 0x5F)]
        public static int LD_A_R(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = cpu.Registers.R;
            cpu.Registers.Sign = cpu.Registers.A.IsNegative();
            cpu.Registers.Zero = cpu.Registers.A == 0;
            cpu.Registers.HalfCarry = false;

            // TODO: Handle the case where an interrupt occurs during this instruction (parity should be 0)
            cpu.Registers.ParityOrOverflow = cpu.Registers.IFF2;
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();

            return 9;
        }

        [ExtendedInstruction("LD I, A", 1, 0xED, 0x47)]
        public static int LD_I_A(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.I = cpu.Registers.A;
            cpu.ControlLines.SystemClock.Tick();

            return 9;
        }

        [ExtendedInstruction("LD R, A", 1, 0xED, 0x4F)]
        public static int LD_R_A(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.R = cpu.Registers.A;
            cpu.ControlLines.SystemClock.Tick();

            return 9;
        }
        #endregion

        #region 16-bit Loads
        [MainInstruction("LD BC, nn", 3, 0x01)]
        [MainInstruction("LD DE, nn", 3, 0x11)]
        [MainInstruction("LD HL, nn", 3, 0x21)]
        [MainInstruction("LD SP, nn", 3, 0x31)]
        public static int LD_dd_nn(IZ80CPU cpu, byte[] instruction)
        {
            var dst = instruction[0].ExtractBits(4, 2);
            var nn = Utilities.LETo16Bit(instruction[1], instruction[2]);
            WriteWordToCpuRegister_BC_DE_HL_SP(cpu, dst, nn);

            return 10;
        }

        [IXInstruction("LD IX, nn", 4, 0xDD, 0x21)]
        public static int LD_IX_nn(IZ80CPU cpu, byte[] instruction)
        {
            var nn = Utilities.LETo16Bit(instruction[2], instruction[3]);
            cpu.Registers.IX = nn;

            return 14;
        }

        [IYInstruction("LD IY, nn", 4, 0xFD, 0x21)]
        public static int LD_IY_nn(IZ80CPU cpu, byte[] instruction)
        {
            var nn = Utilities.LETo16Bit(instruction[2], instruction[3]);
            cpu.Registers.IY = nn;

            return 14;
        }

        [MainInstruction("LD HL, (nn)", 3, 0x2A)]
        public static int LD_HL_nn_mem(IZ80CPU cpu, byte[] instruction)
        {
            var nn = Utilities.LETo16Bit(instruction[1], instruction[2]);
            cpu.Registers.HL = cpu.ReadWord(nn);

            return 16;
        }

        [ExtendedInstruction("LD BC, (nn)", 4, 0xED, 0x4B)]
        [ExtendedInstruction("LD DE, (nn)", 4, 0xED, 0x5B)]
        [ExtendedInstruction("LD HL, (nn)", 4, 0xED, 0x6B)]
        [ExtendedInstruction("LD SP, (nn)", 4, 0xED, 0x7B)]
        public static int LD_dd_nn_mem(IZ80CPU cpu, byte[] instruction)
        {
            var dst = instruction[1].ExtractBits(4, 2);
            var nn = Utilities.LETo16Bit(instruction[2], instruction[3]);
            var word = cpu.ReadWord(nn);

            WriteWordToCpuRegister_BC_DE_HL_SP(cpu, dst, word);

            return 20;
        }

        [IXInstruction("LD IX, (nn)", 4, 0xDD, 0x2A)]
        public static int LD_IX_nn_mem(IZ80CPU cpu, byte[] instruction)
        {
            var nn = Utilities.LETo16Bit(instruction[2], instruction[3]);
            cpu.Registers.IX = cpu.ReadWord(nn);
            return 20;
        }

        [IYInstruction("LD IY, (nn)", 4, 0xFD, 0x2A)]
        public static int LD_IY_nn_mem(IZ80CPU cpu, byte[] instruction)
        {
            var nn = Utilities.LETo16Bit(instruction[2], instruction[3]);
            cpu.Registers.IY = cpu.ReadWord(nn);
            return 20;
        }

        [MainInstruction("LD (nn), HL", 3, 0x22)]
        public static int LD_nn_mem_HL(IZ80CPU cpu, byte[] instruction)
        {
            var addr = Utilities.LETo16Bit(instruction[1], instruction[2]);
            cpu.WriteWord(addr, cpu.Registers.HL);

            return 16;
        }

        [ExtendedInstruction("LD (nn), BC", 4, 0xED, 0x43)]
        [ExtendedInstruction("LD (nn), DE", 4, 0xED, 0x53)]
        [ExtendedInstruction("LD (nn), HL", 4, 0xED, 0x63)]
        [ExtendedInstruction("LD (nn), SP", 4, 0xED, 0x73)]
        public static int LD_nn_mem_dd(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[1].ExtractBits(4, 2);
            var addr = Utilities.LETo16Bit(instruction[2], instruction[3]);
            var data = ReadWordFromCpuRegister_BC_DE_HL_SP(cpu, src);
            cpu.WriteWord(addr, data);

            return 20;
        }

        [IXInstruction("LD (nn), IX", 4, 0xDD, 0x22)]
        public static int LD_nn_mem_IX(IZ80CPU cpu, byte[] instruction)
        {
            var addr = Utilities.LETo16Bit(instruction[2], instruction[3]);
            cpu.WriteWord(addr, cpu.Registers.IX);

            return 20;
        }

        [IYInstruction("LD (nn), IY", 4, 0xFD, 0x22)]
        public static int LD_nn_mem_IY(IZ80CPU cpu, byte[] instruction)
        {
            var addr = Utilities.LETo16Bit(instruction[2], instruction[3]);
            cpu.WriteWord(addr, cpu.Registers.IY);

            return 20;
        }

        [MainInstruction("LD SP, HL", 1, 0xF9)]
        public static int LD_SP_HL(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.SP = cpu.Registers.HL;

            cpu.ControlLines.SystemClock.TickMultiple(2);
            return 6;
        }

        [IXInstruction("LD SP, IX", 2, 0xDD, 0xF9)]
        public static int LD_SP_IX(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.SP = cpu.Registers.IX;

            cpu.ControlLines.SystemClock.TickMultiple(2);
            return 10;
        }

        [IYInstruction("LD SP, IY", 2, 0xFD, 0xF9)]
        public static int LD_SP_IY(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.SP = cpu.Registers.IY;

            cpu.ControlLines.SystemClock.TickMultiple(2);
            return 10;
        }
        #endregion

        #region Stack

        [MainInstruction("PUSH BC", 1, 0xC5)]
        [MainInstruction("PUSH DE", 1, 0xD5)]
        [MainInstruction("PUSH HL", 1, 0xE5)]
        [MainInstruction("PUSH AF", 1, 0xF5)]
        public static int PUSH_qq(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(4, 2);
            var data = ReadWordFromCpuRegister_BC_DE_HL_AF(cpu, src);

            cpu.ControlLines.SystemClock.TickMultiple(2);
            cpu.PushWord(data);

            return 11;
        }

        [IXInstruction("PUSH IX", 2, 0xDD, 0xE5)]
        public static int PUSH_IX(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            cpu.PushWord(cpu.Registers.IX);

            return 15;
        }

        [IYInstruction("PUSH IY", 2, 0xFD, 0xE5)]
        public static int PUSH_IY(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            cpu.PushWord(cpu.Registers.IY);

            return 15;
        }

        [MainInstruction("POP BC", 1, 0xC1)]
        [MainInstruction("POP DE", 1, 0xD1)]
        [MainInstruction("POP HL", 1, 0xE1)]
        [MainInstruction("POP AF", 1, 0xF1)]
        public static int POP_qq(IZ80CPU cpu, byte[] instruction)
        {
            var dst = instruction[0].ExtractBits(4, 2);

            var lower = cpu.PopByte();
            var word = ReadWordFromCpuRegister_BC_DE_HL_AF(cpu, dst).SetLowerByte(lower);
            WriteWordToCpuRegister_BC_DE_HL_AF(cpu, dst, word);

            var upper = cpu.PopByte();
            word = word.SetUpperByte(upper);
            WriteWordToCpuRegister_BC_DE_HL_AF(cpu, dst, word);

            return 10;
        }

        [IXInstruction("POP IX", 2, 0xDD, 0xE1)]
        public static int POP_IX(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.IXLower = cpu.PopByte();
            cpu.Registers.IXUpper = cpu.PopByte();

            return 14;
        }

        [IYInstruction("POP IY", 2, 0xDD, 0xE1)]
        public static int POP_IY(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.IYLower = cpu.PopByte();
            cpu.Registers.IYUpper = cpu.PopByte();

            return 14;
        }
        #endregion

    }
}
