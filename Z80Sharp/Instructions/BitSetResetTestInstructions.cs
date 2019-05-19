using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        [BitInstruction("BIT 0, A", 2, 0xCB, 0x47)]
        [BitInstruction("BIT 1, A", 2, 0xCB, 0x4F)]
        [BitInstruction("BIT 2, A", 2, 0xCB, 0x57)]
        [BitInstruction("BIT 3, A", 2, 0xCB, 0x5F)]
        [BitInstruction("BIT 4, A", 2, 0xCB, 0x67)]
        [BitInstruction("BIT 5, A", 2, 0xCB, 0x6F)]
        [BitInstruction("BIT 6, A", 2, 0xCB, 0x77)]
        [BitInstruction("BIT 7, A", 2, 0xCB, 0x7F)]
        [BitInstruction("BIT 0, B", 2, 0xCB, 0x40)]
        [BitInstruction("BIT 1, B", 2, 0xCB, 0x48)]
        [BitInstruction("BIT 2, B", 2, 0xCB, 0x50)]
        [BitInstruction("BIT 3, B", 2, 0xCB, 0x58)]
        [BitInstruction("BIT 4, B", 2, 0xCB, 0x60)]
        [BitInstruction("BIT 5, B", 2, 0xCB, 0x68)]
        [BitInstruction("BIT 6, B", 2, 0xCB, 0x70)]
        [BitInstruction("BIT 7, B", 2, 0xCB, 0x78)]
        [BitInstruction("BIT 0, C", 2, 0xCB, 0x41)]
        [BitInstruction("BIT 1, C", 2, 0xCB, 0x49)]
        [BitInstruction("BIT 2, C", 2, 0xCB, 0x51)]
        [BitInstruction("BIT 3, C", 2, 0xCB, 0x59)]
        [BitInstruction("BIT 4, C", 2, 0xCB, 0x61)]
        [BitInstruction("BIT 5, C", 2, 0xCB, 0x69)]
        [BitInstruction("BIT 6, C", 2, 0xCB, 0x71)]
        [BitInstruction("BIT 7, C", 2, 0xCB, 0x79)]
        [BitInstruction("BIT 0, D", 2, 0xCB, 0x42)]
        [BitInstruction("BIT 1, D", 2, 0xCB, 0x4A)]
        [BitInstruction("BIT 2, D", 2, 0xCB, 0x52)]
        [BitInstruction("BIT 3, D", 2, 0xCB, 0x5A)]
        [BitInstruction("BIT 4, D", 2, 0xCB, 0x62)]
        [BitInstruction("BIT 5, D", 2, 0xCB, 0x6A)]
        [BitInstruction("BIT 6, D", 2, 0xCB, 0x72)]
        [BitInstruction("BIT 7, D", 2, 0xCB, 0x7A)]
        [BitInstruction("BIT 0, E", 2, 0xCB, 0x43)]
        [BitInstruction("BIT 1, E", 2, 0xCB, 0x4B)]
        [BitInstruction("BIT 2, E", 2, 0xCB, 0x53)]
        [BitInstruction("BIT 3, E", 2, 0xCB, 0x5B)]
        [BitInstruction("BIT 4, E", 2, 0xCB, 0x63)]
        [BitInstruction("BIT 5, E", 2, 0xCB, 0x6B)]
        [BitInstruction("BIT 6, E", 2, 0xCB, 0x73)]
        [BitInstruction("BIT 7, E", 2, 0xCB, 0x7B)]
        [BitInstruction("BIT 0, H", 2, 0xCB, 0x44)]
        [BitInstruction("BIT 1, H", 2, 0xCB, 0x4C)]
        [BitInstruction("BIT 2, H", 2, 0xCB, 0x54)]
        [BitInstruction("BIT 3, H", 2, 0xCB, 0x5C)]
        [BitInstruction("BIT 4, H", 2, 0xCB, 0x64)]
        [BitInstruction("BIT 5, H", 2, 0xCB, 0x6C)]
        [BitInstruction("BIT 6, H", 2, 0xCB, 0x74)]
        [BitInstruction("BIT 7, H", 2, 0xCB, 0x7C)]
        [BitInstruction("BIT 0, L", 2, 0xCB, 0x45)]
        [BitInstruction("BIT 1, L", 2, 0xCB, 0x4D)]
        [BitInstruction("BIT 2, L", 2, 0xCB, 0x55)]
        [BitInstruction("BIT 3, L", 2, 0xCB, 0x5D)]
        [BitInstruction("BIT 4, L", 2, 0xCB, 0x65)]
        [BitInstruction("BIT 5, L", 2, 0xCB, 0x6D)]
        [BitInstruction("BIT 6, L", 2, 0xCB, 0x75)]
        [BitInstruction("BIT 7, L", 2, 0xCB, 0x7D)]
        public static int BIT_b_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var bit = instruction[1].ExtractBits(3, 3);
            var val = ReadByteFromCpuRegister(cpu, reg);

            cpu.Registers.Zero = !val.GetBit(bit);
            cpu.Registers.HalfCarry = true;
            cpu.Registers.Subtract = false;

            return 8;
        }


        [BitInstruction("BIT 0, (HL)", 2, 0xCB, 0x46)]
        [BitInstruction("BIT 1, (HL)", 2, 0xCB, 0x4E)]
        [BitInstruction("BIT 2, (HL)", 2, 0xCB, 0x56)]
        [BitInstruction("BIT 3, (HL)", 2, 0xCB, 0x5E)]
        [BitInstruction("BIT 4, (HL)", 2, 0xCB, 0x66)]
        [BitInstruction("BIT 5, (HL)", 2, 0xCB, 0x6E)]
        [BitInstruction("BIT 6, (HL)", 2, 0xCB, 0x76)]
        [BitInstruction("BIT 7, (HL)", 2, 0xCB, 0x7E)]
        public static int BIT_b_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var bit = instruction[1].ExtractBits(3, 3);
            var val = cpu.ReadMemory(cpu.Registers.HL);

            cpu.Registers.Zero = !val.GetBit(bit);
            cpu.Registers.HalfCarry = true;
            cpu.Registers.Subtract = false;

            return 12;
        }

        [IXBitInstruction("BIT 0, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x46)]
        [IXBitInstruction("BIT 1, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x4E)]
        [IXBitInstruction("BIT 2, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x56)]
        [IXBitInstruction("BIT 3, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x5E)]
        [IXBitInstruction("BIT 4, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x66)]
        [IXBitInstruction("BIT 5, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x6E)]
        [IXBitInstruction("BIT 6, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x76)]
        [IXBitInstruction("BIT 7, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x7E)]
        public static int BIT_b_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var bit = instruction[1].ExtractBits(3, 3);
            var val = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]), 1);

            cpu.Registers.Zero = !val.GetBit(bit);
            cpu.Registers.HalfCarry = true;
            cpu.Registers.Subtract = false;

            return 20;
        }

        [IYBitInstruction("BIT 0, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x46)]
        [IYBitInstruction("BIT 1, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x4E)]
        [IYBitInstruction("BIT 2, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x56)]
        [IYBitInstruction("BIT 3, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x5E)]
        [IYBitInstruction("BIT 4, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x66)]
        [IYBitInstruction("BIT 5, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x6E)]
        [IYBitInstruction("BIT 6, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x76)]
        [IYBitInstruction("BIT 7, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x7E)]
        public static int BIT_b_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var bit = instruction[1].ExtractBits(3, 3);
            var val = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]), 1);

            cpu.Registers.Zero = !val.GetBit(bit);
            cpu.Registers.HalfCarry = true;
            cpu.Registers.Subtract = false;

            return 20;
        }

        [BitInstruction("SET 0, A", 2, 0xCB, 0xC7)]
        [BitInstruction("SET 1, A", 2, 0xCB, 0xCF)]
        [BitInstruction("SET 2, A", 2, 0xCB, 0xD7)]
        [BitInstruction("SET 3, A", 2, 0xCB, 0xDF)]
        [BitInstruction("SET 4, A", 2, 0xCB, 0xE7)]
        [BitInstruction("SET 5, A", 2, 0xCB, 0xEF)]
        [BitInstruction("SET 6, A", 2, 0xCB, 0xF7)]
        [BitInstruction("SET 7, A", 2, 0xCB, 0xFF)]
        [BitInstruction("SET 0, B", 2, 0xCB, 0xC0)]
        [BitInstruction("SET 1, B", 2, 0xCB, 0xC8)]
        [BitInstruction("SET 2, B", 2, 0xCB, 0xD0)]
        [BitInstruction("SET 3, B", 2, 0xCB, 0xD8)]
        [BitInstruction("SET 4, B", 2, 0xCB, 0xE0)]
        [BitInstruction("SET 5, B", 2, 0xCB, 0xE8)]
        [BitInstruction("SET 6, B", 2, 0xCB, 0xF0)]
        [BitInstruction("SET 7, B", 2, 0xCB, 0xF8)]
        [BitInstruction("SET 0, C", 2, 0xCB, 0xC1)]
        [BitInstruction("SET 1, C", 2, 0xCB, 0xC9)]
        [BitInstruction("SET 2, C", 2, 0xCB, 0xD1)]
        [BitInstruction("SET 3, C", 2, 0xCB, 0xD9)]
        [BitInstruction("SET 4, C", 2, 0xCB, 0xE1)]
        [BitInstruction("SET 5, C", 2, 0xCB, 0xE9)]
        [BitInstruction("SET 6, C", 2, 0xCB, 0xF1)]
        [BitInstruction("SET 7, C", 2, 0xCB, 0xF9)]
        [BitInstruction("SET 0, D", 2, 0xCB, 0xC2)]
        [BitInstruction("SET 1, D", 2, 0xCB, 0xCA)]
        [BitInstruction("SET 2, D", 2, 0xCB, 0xD2)]
        [BitInstruction("SET 3, D", 2, 0xCB, 0xDA)]
        [BitInstruction("SET 4, D", 2, 0xCB, 0xE2)]
        [BitInstruction("SET 5, D", 2, 0xCB, 0xEA)]
        [BitInstruction("SET 6, D", 2, 0xCB, 0xF2)]
        [BitInstruction("SET 7, D", 2, 0xCB, 0xFA)]
        [BitInstruction("SET 0, E", 2, 0xCB, 0xC3)]
        [BitInstruction("SET 1, E", 2, 0xCB, 0xCB)]
        [BitInstruction("SET 2, E", 2, 0xCB, 0xD3)]
        [BitInstruction("SET 3, E", 2, 0xCB, 0xDB)]
        [BitInstruction("SET 4, E", 2, 0xCB, 0xE3)]
        [BitInstruction("SET 5, E", 2, 0xCB, 0xEB)]
        [BitInstruction("SET 6, E", 2, 0xCB, 0xF3)]
        [BitInstruction("SET 7, E", 2, 0xCB, 0xFB)]
        [BitInstruction("SET 0, H", 2, 0xCB, 0xC4)]
        [BitInstruction("SET 1, H", 2, 0xCB, 0xCC)]
        [BitInstruction("SET 2, H", 2, 0xCB, 0xD4)]
        [BitInstruction("SET 3, H", 2, 0xCB, 0xDC)]
        [BitInstruction("SET 4, H", 2, 0xCB, 0xE4)]
        [BitInstruction("SET 5, H", 2, 0xCB, 0xEC)]
        [BitInstruction("SET 6, H", 2, 0xCB, 0xF4)]
        [BitInstruction("SET 7, H", 2, 0xCB, 0xFC)]
        [BitInstruction("SET 0, L", 2, 0xCB, 0xC5)]
        [BitInstruction("SET 1, L", 2, 0xCB, 0xCD)]
        [BitInstruction("SET 2, L", 2, 0xCB, 0xD5)]
        [BitInstruction("SET 3, L", 2, 0xCB, 0xDD)]
        [BitInstruction("SET 4, L", 2, 0xCB, 0xE5)]
        [BitInstruction("SET 5, L", 2, 0xCB, 0xED)]
        [BitInstruction("SET 6, L", 2, 0xCB, 0xF5)]
        [BitInstruction("SET 7, L", 2, 0xCB, 0xFD)]
        public static int SET_b_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var bit = instruction[1].ExtractBits(3, 3);
            var val = ReadByteFromCpuRegister(cpu, reg);
            WriteByteToCpuRegister(cpu, reg, val.SetBit(true, bit));

            return 8;
        }


        [BitInstruction("SET 0, (HL)", 2, 0xCB, 0xC6)]
        [BitInstruction("SET 1, (HL)", 2, 0xCB, 0xCE)]
        [BitInstruction("SET 2, (HL)", 2, 0xCB, 0xD6)]
        [BitInstruction("SET 3, (HL)", 2, 0xCB, 0xDE)]
        [BitInstruction("SET 4, (HL)", 2, 0xCB, 0xE6)]
        [BitInstruction("SET 5, (HL)", 2, 0xCB, 0xEE)]
        [BitInstruction("SET 6, (HL)", 2, 0xCB, 0xF6)]
        [BitInstruction("SET 7, (HL)", 2, 0xCB, 0xFE)]
        public static int SET_b_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var bit = instruction[1].ExtractBits(3, 3);
            var val = cpu.ReadMemory(cpu.Registers.HL, 1);
            val = val.SetBit(true, bit);
            cpu.WriteMemory(cpu.Registers.HL, val);

            return 15;
        }

        [IXBitInstruction("SET 0, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xC6)]
        [IXBitInstruction("SET 1, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xCE)]
        [IXBitInstruction("SET 2, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xD6)]
        [IXBitInstruction("SET 3, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xDE)]
        [IXBitInstruction("SET 4, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xE6)]
        [IXBitInstruction("SET 5, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xEE)]
        [IXBitInstruction("SET 6, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xF6)]
        [IXBitInstruction("SET 7, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xFE)]
        public static int SET_b_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var bit = instruction[1].ExtractBits(3, 3);
            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var val = cpu.ReadMemory(addr, 1);
            val = val.SetBit(true, bit);
            
            cpu.WriteMemory(addr, val);

            return 23;
        }

        [IYBitInstruction("SET 0, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xC6)]
        [IYBitInstruction("SET 1, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xCE)]
        [IYBitInstruction("SET 2, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xD6)]
        [IYBitInstruction("SET 3, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xDE)]
        [IYBitInstruction("SET 4, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xE6)]
        [IYBitInstruction("SET 5, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xEE)]
        [IYBitInstruction("SET 6, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xF6)]
        [IYBitInstruction("SET 7, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xFE)]
        public static int SET_b_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var bit = instruction[1].ExtractBits(3, 3);
            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var val = cpu.ReadMemory(addr, 1);
            val = val.SetBit(true, bit);
            
            cpu.WriteMemory(addr, val);

            return 23;
        }

        [BitInstruction("RES 0, A", 2, 0xCB, 0x87)]
        [BitInstruction("RES 1, A", 2, 0xCB, 0x8F)]
        [BitInstruction("RES 2, A", 2, 0xCB, 0x97)]
        [BitInstruction("RES 3, A", 2, 0xCB, 0x9F)]
        [BitInstruction("RES 4, A", 2, 0xCB, 0xA7)]
        [BitInstruction("RES 5, A", 2, 0xCB, 0xAF)]
        [BitInstruction("RES 6, A", 2, 0xCB, 0xB7)]
        [BitInstruction("RES 7, A", 2, 0xCB, 0xBF)]
        [BitInstruction("RES 0, B", 2, 0xCB, 0x80)]
        [BitInstruction("RES 1, B", 2, 0xCB, 0x88)]
        [BitInstruction("RES 2, B", 2, 0xCB, 0x90)]
        [BitInstruction("RES 3, B", 2, 0xCB, 0x98)]
        [BitInstruction("RES 4, B", 2, 0xCB, 0xA0)]
        [BitInstruction("RES 5, B", 2, 0xCB, 0xA8)]
        [BitInstruction("RES 6, B", 2, 0xCB, 0xB0)]
        [BitInstruction("RES 7, B", 2, 0xCB, 0xB8)]
        [BitInstruction("RES 0, C", 2, 0xCB, 0x81)]
        [BitInstruction("RES 1, C", 2, 0xCB, 0x89)]
        [BitInstruction("RES 2, C", 2, 0xCB, 0x91)]
        [BitInstruction("RES 3, C", 2, 0xCB, 0x99)]
        [BitInstruction("RES 4, C", 2, 0xCB, 0xA1)]
        [BitInstruction("RES 5, C", 2, 0xCB, 0xA9)]
        [BitInstruction("RES 6, C", 2, 0xCB, 0xB1)]
        [BitInstruction("RES 7, C", 2, 0xCB, 0xB9)]
        [BitInstruction("RES 0, D", 2, 0xCB, 0x82)]
        [BitInstruction("RES 1, D", 2, 0xCB, 0x8A)]
        [BitInstruction("RES 2, D", 2, 0xCB, 0x92)]
        [BitInstruction("RES 3, D", 2, 0xCB, 0x9A)]
        [BitInstruction("RES 4, D", 2, 0xCB, 0xA2)]
        [BitInstruction("RES 5, D", 2, 0xCB, 0xAA)]
        [BitInstruction("RES 6, D", 2, 0xCB, 0xB2)]
        [BitInstruction("RES 7, D", 2, 0xCB, 0xBA)]
        [BitInstruction("RES 0, E", 2, 0xCB, 0x83)]
        [BitInstruction("RES 1, E", 2, 0xCB, 0x8B)]
        [BitInstruction("RES 2, E", 2, 0xCB, 0x93)]
        [BitInstruction("RES 3, E", 2, 0xCB, 0x9B)]
        [BitInstruction("RES 4, E", 2, 0xCB, 0xA3)]
        [BitInstruction("RES 5, E", 2, 0xCB, 0xAB)]
        [BitInstruction("RES 6, E", 2, 0xCB, 0xB3)]
        [BitInstruction("RES 7, E", 2, 0xCB, 0xBB)]
        [BitInstruction("RES 0, H", 2, 0xCB, 0x84)]
        [BitInstruction("RES 1, H", 2, 0xCB, 0x8C)]
        [BitInstruction("RES 2, H", 2, 0xCB, 0x94)]
        [BitInstruction("RES 3, H", 2, 0xCB, 0x9C)]
        [BitInstruction("RES 4, H", 2, 0xCB, 0xA4)]
        [BitInstruction("RES 5, H", 2, 0xCB, 0xAC)]
        [BitInstruction("RES 6, H", 2, 0xCB, 0xB4)]
        [BitInstruction("RES 7, H", 2, 0xCB, 0xBC)]
        [BitInstruction("RES 0, L", 2, 0xCB, 0x85)]
        [BitInstruction("RES 1, L", 2, 0xCB, 0x8D)]
        [BitInstruction("RES 2, L", 2, 0xCB, 0x95)]
        [BitInstruction("RES 3, L", 2, 0xCB, 0x9D)]
        [BitInstruction("RES 4, L", 2, 0xCB, 0xA5)]
        [BitInstruction("RES 5, L", 2, 0xCB, 0xAD)]
        [BitInstruction("RES 6, L", 2, 0xCB, 0xB5)]
        [BitInstruction("RES 7, L", 2, 0xCB, 0xBD)]
        public static int RES_b_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var bit = instruction[1].ExtractBits(3, 3);
            var val = ReadByteFromCpuRegister(cpu, reg);
            WriteByteToCpuRegister(cpu, reg, val.SetBit(false, bit));

            return 8;
        }


        [BitInstruction("RES 0, (HL)", 2, 0xCB, 0x86)]
        [BitInstruction("RES 1, (HL)", 2, 0xCB, 0x8E)]
        [BitInstruction("RES 2, (HL)", 2, 0xCB, 0x96)]
        [BitInstruction("RES 3, (HL)", 2, 0xCB, 0x9E)]
        [BitInstruction("RES 4, (HL)", 2, 0xCB, 0xA6)]
        [BitInstruction("RES 5, (HL)", 2, 0xCB, 0xAE)]
        [BitInstruction("RES 6, (HL)", 2, 0xCB, 0xB6)]
        [BitInstruction("RES 7, (HL)", 2, 0xCB, 0xBE)]
        public static int RES_b_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var bit = instruction[1].ExtractBits(3, 3);
            var val = cpu.ReadMemory(cpu.Registers.HL, 1);
            val = val.SetBit(false, bit);
            cpu.WriteMemory(cpu.Registers.HL, val);

            return 15;
        }

        [IXBitInstruction("RES 0, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x86)]
        [IXBitInstruction("RES 1, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x8E)]
        [IXBitInstruction("RES 2, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x96)]
        [IXBitInstruction("RES 3, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x9E)]
        [IXBitInstruction("RES 4, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xA6)]
        [IXBitInstruction("RES 5, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xAE)]
        [IXBitInstruction("RES 6, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xB6)]
        [IXBitInstruction("RES 7, (IX+d)", 4, 0xDD, 0xCB, 0x00, 0xBE)]
        public static int RES_b_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var bit = instruction[1].ExtractBits(3, 3);
            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var val = cpu.ReadMemory(addr, 1);
            val = val.SetBit(false, bit);
            
            cpu.WriteMemory(addr, val);

            return 23;
        }

        [IYBitInstruction("RES 0, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x86)]
        [IYBitInstruction("RES 1, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x8E)]
        [IYBitInstruction("RES 2, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x96)]
        [IYBitInstruction("RES 3, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x9E)]
        [IYBitInstruction("RES 4, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xA6)]
        [IYBitInstruction("RES 5, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xAE)]
        [IYBitInstruction("RES 6, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xB6)]
        [IYBitInstruction("RES 7, (IY+d)", 4, 0xFD, 0xCB, 0x00, 0xBE)]
        public static int RES_b_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var bit = instruction[1].ExtractBits(3, 3);
            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var val = cpu.ReadMemory(addr, 1);
            val = val.SetBit(false, bit);
            
            cpu.WriteMemory(addr, val);

            return 23;
        }
    }
}
