using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public partial class Z80Instructions
    {
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
    }
}
