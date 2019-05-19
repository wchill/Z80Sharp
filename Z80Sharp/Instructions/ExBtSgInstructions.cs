using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        #region Exchange

        [MainInstruction("EX DE, HL", 1, 0xEB)]
        public static int EX_DE_HL(IZ80CPU cpu, byte[] instruction)
        {
            var temp = cpu.Registers.DE;
            cpu.Registers.DE = cpu.Registers.HL;
            cpu.Registers.HL = temp;

            return 4;
        }

        [MainInstruction("EX AF, AF'", 1, 0x08)]
        public static int EX_AF_AFp(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.SwapAFWithShadow();
            return 4;
        }

        [MainInstruction("EXX", 1, 0xD9)]
        public static int EXX(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.SwapRegisterPairsWithShadow();
            return 4;
        }

        [MainInstruction("EX (SP), HL", 1, 0xE3)]
        public static int EX_SP_mem_HL(IZ80CPU cpu, byte[] instruction)
        {
            var lower = cpu.ReadMemory(cpu.Registers.SP);
            var upper = cpu.ReadMemory((ushort) (cpu.Registers.SP + 1));
            cpu.ControlLines.SystemClock.Tick();

            cpu.WriteMemory(cpu.Registers.SP, cpu.Registers.L);
            cpu.Registers.L = lower;
            cpu.WriteMemory((ushort)(cpu.Registers.SP + 1), cpu.Registers.H);
            cpu.Registers.H = upper;
            cpu.ControlLines.SystemClock.TickMultiple(2);

            return 19;
        }

        [IXInstruction("EX (SP), IX", 2, 0xDD, 0xE3)]
        public static int EX_SP_mem_IX(IZ80CPU cpu, byte[] instruction)
        {
            var lower = cpu.ReadMemory(cpu.Registers.SP);
            var upper = cpu.ReadMemory((ushort)(cpu.Registers.SP + 1));
            cpu.ControlLines.SystemClock.Tick();

            cpu.WriteMemory(cpu.Registers.SP, cpu.Registers.IXLower);
            cpu.Registers.IXLower = lower;
            cpu.WriteMemory((ushort)(cpu.Registers.SP + 1), cpu.Registers.IXUpper);
            cpu.Registers.IXUpper = upper;
            cpu.ControlLines.SystemClock.TickMultiple(2);

            return 23;
        }

        [IYInstruction("EX (SP), IY", 2, 0xDD, 0xE3)]
        public static int EX_SP_mem_IY(IZ80CPU cpu, byte[] instruction)
        {
            var lower = cpu.ReadMemory(cpu.Registers.SP);
            var upper = cpu.ReadMemory((ushort)(cpu.Registers.SP + 1));
            cpu.ControlLines.SystemClock.Tick();

            cpu.WriteMemory(cpu.Registers.SP, cpu.Registers.IYLower);
            cpu.Registers.IYLower = lower;
            cpu.WriteMemory((ushort)(cpu.Registers.SP + 1), cpu.Registers.IYUpper);
            cpu.Registers.IYUpper = upper;
            cpu.ControlLines.SystemClock.TickMultiple(2);

            return 23;
        }

        #endregion

        #region Block Transfer

        [ExtendedInstruction("LDI", 2, 0xED, 0xA0)]
        public static int LDI(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.WriteMemory(cpu.Registers.DE, data);
            cpu.Registers.HL++;
            cpu.Registers.DE++;
            cpu.Registers.BC--;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.TickMultiple(2);
            return 16;
        }

        [ExtendedInstruction("LDIR", 2, 0xED, 0xB0)]
        public static int LDIR(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.WriteMemory(cpu.Registers.DE, data);
            cpu.Registers.HL++;
            cpu.Registers.DE++;
            cpu.Registers.BC--;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.TickMultiple(2);

            if (cpu.Registers.BC != 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 21;
        }

        [ExtendedInstruction("LDD", 2, 0xED, 0xA8)]
        public static int LDD(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.WriteMemory(cpu.Registers.DE, data);
            cpu.Registers.HL--;
            cpu.Registers.DE--;
            cpu.Registers.BC--;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.TickMultiple(2);
            return 16;
        }

        [ExtendedInstruction("LDDR", 2, 0xED, 0xB8)]
        public static int LDDR(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.WriteMemory(cpu.Registers.DE, data);
            cpu.Registers.HL--;
            cpu.Registers.DE--;
            cpu.Registers.BC--;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.TickMultiple(2);

            if (cpu.Registers.BC != 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 21;
        }

        #endregion

        #region Search Group

        [ExtendedInstruction("CPI", 2, 0xED, 0xA1)]
        public static int CPI(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            var cmp = cpu.Registers.A - data;
            cpu.Registers.HL++;
            cpu.Registers.BC--;

            cpu.Registers.Sign = cmp < 0;
            cpu.Registers.Zero = cmp == 0;
            cpu.Registers.HalfCarry = cpu.Registers.A.WillHalfCarry(data);
            cpu.Registers.Subtract = true;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;

            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 16;
        }

        [ExtendedInstruction("CPIR", 2, 0xED, 0xB1)]
        public static int CPIR(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            var cmp = cpu.Registers.A - data;
            cpu.Registers.HL++;
            cpu.Registers.BC--;

            cpu.Registers.Sign = cmp < 0;
            cpu.Registers.Zero = cmp == 0;
            cpu.Registers.HalfCarry = cpu.Registers.A.WillHalfCarry(data);
            cpu.Registers.Subtract = true;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;

            cpu.ControlLines.SystemClock.TickMultiple(5);

            if (cpu.Registers.Zero || cpu.Registers.BC == 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 21;
        }

        [ExtendedInstruction("CPD", 2, 0xED, 0xA9)]
        public static int CPD(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            var cmp = cpu.Registers.A - data;
            cpu.Registers.HL--;
            cpu.Registers.BC--;

            cpu.Registers.Sign = cmp < 0;
            cpu.Registers.Zero = cmp == 0;
            cpu.Registers.HalfCarry = cpu.Registers.A.WillHalfCarry(data);
            cpu.Registers.Subtract = true;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;

            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 16;
        }

        [ExtendedInstruction("CPDR", 2, 0xED, 0xB9)]
        public static int CPDR(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            var cmp = cpu.Registers.A - data;
            cpu.Registers.HL--;
            cpu.Registers.BC--;

            cpu.Registers.Sign = cmp < 0;
            cpu.Registers.Zero = cmp == 0;
            cpu.Registers.HalfCarry = cpu.Registers.A.WillHalfCarry(data);
            cpu.Registers.Subtract = true;
            cpu.Registers.ParityOrOverflow = cpu.Registers.BC != 0;

            cpu.ControlLines.SystemClock.TickMultiple(5);

            if (cpu.Registers.Zero || cpu.Registers.BC == 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);
            return 21;
        }

        #endregion
    }
}
