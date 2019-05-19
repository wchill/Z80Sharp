using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        [MainInstruction("IN A, (n)", 2, 0xDB)]
        public static int IN_A_n(IZ80CPU cpu, byte[] instruction)
        {
            var portAddr = Utilities.LETo16Bit(instruction[1], cpu.Registers.A);
            cpu.Registers.A = cpu.ReadFromPort(portAddr);

            return 11;
        }

        [ExtendedInstruction("IN A, (C)", 2, 0xED, 0x78)]
        [ExtendedInstruction("IN B, (C)", 2, 0xED, 0x40)]
        [ExtendedInstruction("IN C, (C)", 2, 0xED, 0x48)]
        [ExtendedInstruction("IN D, (C)", 2, 0xED, 0x50)]
        [ExtendedInstruction("IN E, (C)", 2, 0xED, 0x58)]
        [ExtendedInstruction("IN H, (C)", 2, 0xED, 0x60)]
        [ExtendedInstruction("IN L, (C)", 2, 0xED, 0x68)]
        public static int IN_r_C(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(3, 3);
            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            var data = cpu.ReadFromPort(portAddr);

            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;
            WriteByteToCpuRegister(cpu, reg, data);

            return 12;
        }

        [ExtendedInstruction("INI", 2, 0xED, 0xA2)]
        public static int INI(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();

            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            var data = cpu.ReadFromPort(portAddr);

            cpu.WriteMemory(cpu.Registers.HL, data);
            cpu.Registers.B--;
            cpu.Registers.HL++;
            cpu.Registers.Zero = cpu.Registers.B == 0;
            cpu.Registers.Subtract = true;

            return 16;
        }

        [ExtendedInstruction("INIR", 2, 0xED, 0xB2)]
        public static int INIR(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();

            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            var data = cpu.ReadFromPort(portAddr);

            cpu.WriteMemory(cpu.Registers.HL, data);
            cpu.Registers.B--;
            cpu.Registers.HL++;
            cpu.Registers.Zero = true;
            cpu.Registers.Subtract = true;

            if (cpu.Registers.B == 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);

            return 21;
        }

        [ExtendedInstruction("IND", 2, 0xED, 0xAA)]
        public static int IND(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();

            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            var data = cpu.ReadFromPort(portAddr);

            cpu.WriteMemory(cpu.Registers.HL, data);
            cpu.Registers.B--;
            cpu.Registers.HL--;
            cpu.Registers.Zero = cpu.Registers.B == 0;
            cpu.Registers.Subtract = true;

            return 16;
        }

        [ExtendedInstruction("INDR", 2, 0xED, 0xBA)]
        public static int INDR(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();

            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            var data = cpu.ReadFromPort(portAddr);

            cpu.WriteMemory(cpu.Registers.HL, data);
            cpu.Registers.B--;
            cpu.Registers.HL--;
            cpu.Registers.Zero = true;
            cpu.Registers.Subtract = true;

            if (cpu.Registers.B == 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);

            return 21;
        }

        [MainInstruction("OUT (n), A", 2, 0xD3)]
        public static int OUT_n_A(IZ80CPU cpu, byte[] instruction)
        {
            var portAddr = Utilities.LETo16Bit(instruction[1], cpu.Registers.A);
            cpu.WriteToPort(portAddr, cpu.Registers.A);

            return 11;
        }

        [ExtendedInstruction("OUT (C), A", 2, 0xED, 0x79)]
        [ExtendedInstruction("OUT (C), B", 2, 0xED, 0x41)]
        [ExtendedInstruction("OUT (C), C", 2, 0xED, 0x49)]
        [ExtendedInstruction("OUT (C), D", 2, 0xED, 0x51)]
        [ExtendedInstruction("OUT (C), E", 2, 0xED, 0x59)]
        [ExtendedInstruction("OUT (C), H", 2, 0xED, 0x61)]
        [ExtendedInstruction("OUT (C), L", 2, 0xED, 0x69)]
        public static int OUT_C_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(3, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);
            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            cpu.WriteToPort(portAddr, data);

            return 12;
        }

        [ExtendedInstruction("OUTI", 2, 0xED, 0xA3)]
        public static int OUTI(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.Registers.B--;
            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            cpu.WriteToPort(portAddr, data);

            cpu.Registers.HL++;
            cpu.Registers.Zero = cpu.Registers.B == 0;
            cpu.Registers.Subtract = true;

            return 16;
        }

        [ExtendedInstruction("OTIR", 2, 0xED, 0xB3)]
        public static int OTIR(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.Registers.B--;
            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            cpu.WriteToPort(portAddr, data);

            cpu.Registers.HL++;
            cpu.Registers.Zero = true;
            cpu.Registers.Subtract = true;

            if (cpu.Registers.B == 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);

            return 21;
        }

        [ExtendedInstruction("OUTD", 2, 0xED, 0xAB)]
        public static int OUTD(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.Registers.B--;
            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            cpu.WriteToPort(portAddr, data);

            cpu.Registers.HL--;
            cpu.Registers.Zero = cpu.Registers.B == 0;
            cpu.Registers.Subtract = true;

            return 16;
        }

        [ExtendedInstruction("OTDR", 2, 0xED, 0xBB)]
        public static int OTDR(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            cpu.Registers.B--;
            var portAddr = Utilities.LETo16Bit(cpu.Registers.C, cpu.Registers.B);
            cpu.WriteToPort(portAddr, data);

            cpu.Registers.HL--;
            cpu.Registers.Zero = true;
            cpu.Registers.Subtract = true;

            if (cpu.Registers.B == 0) return 16;

            cpu.Registers.PC -= 2;
            cpu.ControlLines.SystemClock.TickMultiple(5);

            return 21;
        }
    }
}
