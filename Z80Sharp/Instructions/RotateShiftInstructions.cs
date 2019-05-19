using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public static partial class Z80Instructions
    {
        #region Rotate

        [MainInstruction("RLCA", 1, 0x07)]
        public static int RLCA(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = RotateByteLeft(cpu, cpu.Registers.A);

            return 4;
        }

        [MainInstruction("RLA", 1, 0x17)]
        public static int RLA(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = RotateByteLeftThroughCarry(cpu, cpu.Registers.A);

            return 4;
        }

        [MainInstruction("RRCA", 1, 0x0F)]
        public static int RRCA(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = RotateByteRight(cpu, cpu.Registers.A);

            return 4;
        }

        [MainInstruction("RRA", 1, 0x1F)]
        public static int RRA(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = RotateByteRightThroughCarry(cpu, cpu.Registers.A);

            return 4;
        }

        [BitInstruction("RLC A", 2, 0xCB, 0x07)]
        [BitInstruction("RLC B", 2, 0xCB, 0x00)]
        [BitInstruction("RLC C", 2, 0xCB, 0x01)]
        [BitInstruction("RLC D", 2, 0xCB, 0x02)]
        [BitInstruction("RLC E", 2, 0xCB, 0x03)]
        [BitInstruction("RLC H", 2, 0xCB, 0x04)]
        [BitInstruction("RLC L", 2, 0xCB, 0x05)]
        public static int RLC_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);

            data = RotateByteLeft(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            WriteByteToCpuRegister(cpu, reg, data);

            return 8;
        }

        [BitInstruction("RLC (HL)", 2, 0xCB, 0x06)]
        public static int RLC_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);

            data = RotateByteLeft(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.WriteMemory(cpu.Registers.HL, data);

            return 15;
        }

        [IXBitInstruction("RLC (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x06)]
        public static int RLC_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteLeft(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [IYBitInstruction("RLC (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x06)]
        public static int RLC_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            
            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteLeft(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [BitInstruction("RL A", 2, 0xCB, 0x17)]
        [BitInstruction("RL B", 2, 0xCB, 0x10)]
        [BitInstruction("RL C", 2, 0xCB, 0x11)]
        [BitInstruction("RL D", 2, 0xCB, 0x12)]
        [BitInstruction("RL E", 2, 0xCB, 0x13)]
        [BitInstruction("RL H", 2, 0xCB, 0x14)]
        [BitInstruction("RL L", 2, 0xCB, 0x15)]
        public static int RL_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);

            data = RotateByteLeftThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            WriteByteToCpuRegister(cpu, reg, data);

            return 8;
        }

        [BitInstruction("RL (HL)", 2, 0xCB, 0x16)]
        public static int RL_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);

            data = RotateByteLeftThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.WriteMemory(cpu.Registers.HL, data);

            return 15;
        }

        [IXBitInstruction("RL (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x16)]
        public static int RL_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            
            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteLeftThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [IYBitInstruction("RL (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x16)]
        public static int RL_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            
            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteLeftThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [BitInstruction("RRC A", 2, 0xCB, 0x0F)]
        [BitInstruction("RRC B", 2, 0xCB, 0x08)]
        [BitInstruction("RRC C", 2, 0xCB, 0x09)]
        [BitInstruction("RRC D", 2, 0xCB, 0x0A)]
        [BitInstruction("RRC E", 2, 0xCB, 0x0B)]
        [BitInstruction("RRC H", 2, 0xCB, 0x0C)]
        [BitInstruction("RRC L", 2, 0xCB, 0x0D)]
        public static int RRC_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);

            data = RotateByteRight(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            WriteByteToCpuRegister(cpu, reg, data);

            return 8;
        }

        [BitInstruction("RRC (HL)", 2, 0xCB, 0x0E)]
        public static int RRC_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);

            data = RotateByteRight(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.WriteMemory(cpu.Registers.HL, data);

            return 15;
        }

        [IXBitInstruction("RRC (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x0E)]
        public static int RRC_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            
            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteRight(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [IYBitInstruction("RRC (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x0E)]
        public static int RRC_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            
            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteRight(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [BitInstruction("RR A", 2, 0xCB, 0x17)]
        [BitInstruction("RR B", 2, 0xCB, 0x10)]
        [BitInstruction("RR C", 2, 0xCB, 0x11)]
        [BitInstruction("RR D", 2, 0xCB, 0x12)]
        [BitInstruction("RR E", 2, 0xCB, 0x13)]
        [BitInstruction("RR H", 2, 0xCB, 0x14)]
        [BitInstruction("RR L", 2, 0xCB, 0x15)]
        public static int RR_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);

            data = RotateByteRightThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            WriteByteToCpuRegister(cpu, reg, data);

            return 8;
        }

        [BitInstruction("RR (HL)", 2, 0xCB, 0x16)]
        public static int RR_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);

            data = RotateByteRightThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.WriteMemory(cpu.Registers.HL, data);

            return 15;
        }

        [IXBitInstruction("RR (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x16)]
        public static int RR_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            
            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteRightThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [IYBitInstruction("RR (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x16)]
        public static int RR_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            
            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            data = RotateByteRightThroughCarry(cpu, data);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [ExtendedInstruction("RLD", 2, 0xED, 0x6F)]
        public static int RLD(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            var lowerA = cpu.Registers.A.GetLowerNibble();
            var acc = cpu.Registers.A.SetLowerNibble(data.GetUpperNibble());

            data = data.SetUpperNibble(data.GetLowerNibble());
            data = data.SetLowerNibble(lowerA);

            cpu.Registers.A = acc;
            cpu.Registers.Sign = acc.IsNegative();
            cpu.Registers.Zero = acc == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = acc.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.TickMultiple(4);
            cpu.WriteMemory(cpu.Registers.HL, data);

            return 18;
        }

        [ExtendedInstruction("RRD", 2, 0xED, 0x67)]
        public static int RRD(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            var lowerA = cpu.Registers.A.GetLowerNibble();
            var acc = cpu.Registers.A.SetLowerNibble(data.GetLowerNibble());

            data = data.SetLowerNibble(data.GetUpperNibble());
            data = data.SetUpperNibble(lowerA);

            cpu.Registers.A = acc;
            cpu.Registers.Sign = acc.IsNegative();
            cpu.Registers.Zero = acc == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = acc.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.TickMultiple(4);
            cpu.WriteMemory(cpu.Registers.HL, data);

            return 18;
        }

        private static byte RotateByteLeft(IZ80CPU cpu, byte val)
        {
            cpu.Registers.HalfCarry = false;
            cpu.Registers.Subtract = false;
            cpu.Registers.Carry = cpu.Registers.A.GetBit(7);
            return (byte)((cpu.Registers.A << 1) | (cpu.Registers.A.GetBitAsByte(7) >> 7));
        }

        private static byte RotateByteLeftThroughCarry(IZ80CPU cpu, byte val)
        {
            cpu.Registers.HalfCarry = false;
            cpu.Registers.Subtract = false;
            var carry = val.GetBit(7);
            var ret = (byte)((val << 1) | (cpu.Registers.Carry ? 1 : 0));
            cpu.Registers.Carry = carry;
            return ret;
        }

        private static byte RotateByteRight(IZ80CPU cpu, byte val)
        {
            cpu.Registers.HalfCarry = false;
            cpu.Registers.Subtract = false;
            cpu.Registers.Carry = val.GetBit(0);
            return (byte)((val >> 1) | (val.GetBitAsByte(0) << 7));
        }

        private static byte RotateByteRightThroughCarry(IZ80CPU cpu, byte val)
        {
            cpu.Registers.HalfCarry = false;
            cpu.Registers.Subtract = false;
            var carry = val.GetBit(0);
            var ret = (byte)((val >> 1) | ((cpu.Registers.Carry ? 1 : 0) << 7));
            cpu.Registers.Carry = carry;
            return ret;
        }

        #endregion

        #region Shift

        [BitInstruction("SLA A", 2, 0xCB, 0x27)]
        [BitInstruction("SLA B", 2, 0xCB, 0x20)]
        [BitInstruction("SLA C", 2, 0xCB, 0x21)]
        [BitInstruction("SLA D", 2, 0xCB, 0x22)]
        [BitInstruction("SLA E", 2, 0xCB, 0x23)]
        [BitInstruction("SLA H", 2, 0xCB, 0x24)]
        [BitInstruction("SLA L", 2, 0xCB, 0x25)]
        public static int SLA_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);

            cpu.Registers.Carry = data.GetBit(7);
            data <<= 1;
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            WriteByteToCpuRegister(cpu, reg, data);

            return 8;
        }

        [BitInstruction("SLA (HL)", 2, 0xCB, 0x26)]
        public static int SLA_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);

            cpu.Registers.Carry = data.GetBit(7);
            data <<= 1;
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.WriteMemory(cpu.Registers.HL, data);

            return 15;
        }

        [IXBitInstruction("SLA (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x26)]
        public static int SLA_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            cpu.Registers.Carry = data.GetBit(7);
            data <<= 1;
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [IYBitInstruction("SLA (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x26)]
        public static int SLA_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            cpu.Registers.Carry = data.GetBit(7);
            data <<= 1;
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [BitInstruction("SRA A", 2, 0xCB, 0x2F)]
        [BitInstruction("SRA B", 2, 0xCB, 0x28)]
        [BitInstruction("SRA C", 2, 0xCB, 0x29)]
        [BitInstruction("SRA D", 2, 0xCB, 0x2A)]
        [BitInstruction("SRA E", 2, 0xCB, 0x2B)]
        [BitInstruction("SRA H", 2, 0xCB, 0x2C)]
        [BitInstruction("SRA L", 2, 0xCB, 0x2D)]
        public static int SRA_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            data |= data.GetBitAsByte(6);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            WriteByteToCpuRegister(cpu, reg, data);

            return 8;
        }

        [BitInstruction("SRA (HL)", 2, 0xCB, 0x2E)]
        public static int SRA_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            data |= data.GetBitAsByte(6);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.WriteMemory(cpu.Registers.HL, data);

            return 15;
        }

        [IXBitInstruction("SRA (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x2E)]
        public static int SRA_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            data |= data.GetBitAsByte(6);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [IYBitInstruction("SRA (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x2E)]
        public static int SRA_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            data |= data.GetBitAsByte(6);
            cpu.Registers.Sign = data.IsNegative();
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [BitInstruction("SRL A", 2, 0xCB, 0x3F)]
        [BitInstruction("SRL B", 2, 0xCB, 0x38)]
        [BitInstruction("SRL C", 2, 0xCB, 0x39)]
        [BitInstruction("SRL D", 2, 0xCB, 0x3A)]
        [BitInstruction("SRL E", 2, 0xCB, 0x3B)]
        [BitInstruction("SRL H", 2, 0xCB, 0x3C)]
        [BitInstruction("SRL L", 2, 0xCB, 0x3D)]
        public static int SRL_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[1].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, reg);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            cpu.Registers.Sign = false;
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            WriteByteToCpuRegister(cpu, reg, data);

            return 8;
        }

        [BitInstruction("SRL (HL)", 2, 0xCB, 0x3E)]
        public static int SRL_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            cpu.Registers.Sign = false;
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.WriteMemory(cpu.Registers.HL, data);

            return 15;
        }

        [IXBitInstruction("SRL (IX+d)", 4, 0xDD, 0xCB, 0x00, 0x3E)]
        public static int SRL_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            cpu.Registers.Sign = false;
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        [IYBitInstruction("SRL (IY+d)", 4, 0xFD, 0xCB, 0x00, 0x3E)]
        public static int SRL_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);

            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);

            cpu.Registers.Carry = data.GetBit(0);
            data >>= 1;
            cpu.Registers.Sign = false;
            cpu.Registers.Zero = data == 0;
            cpu.Registers.HalfCarry = false;
            cpu.Registers.ParityOrOverflow = data.IsParityEven();
            cpu.Registers.Subtract = false;

            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            return 15;
        }

        #endregion
    }
}
