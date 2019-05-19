using System;
using System.Collections.Generic;
using System.Text;
using Z80Sharp.Instructions.Attributes;

namespace Z80Sharp.Instructions
{
    public partial class Z80Instructions
    {
        #region 8-bit operations

        #region Addition
        [MainInstruction("ADD A, A", 1, 0x87)]
        [MainInstruction("ADD A, B", 1, 0x80)]
        [MainInstruction("ADD A, C", 1, 0x81)]
        [MainInstruction("ADD A, D", 1, 0x82)]
        [MainInstruction("ADD A, E", 1, 0x83)]
        [MainInstruction("ADD A, H", 1, 0x84)]
        [MainInstruction("ADD A, L", 1, 0x85)]
        public static int ADD_A_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            AddBytes(cpu, cpu.Registers.A, data);

            return 4;
        }

        [MainInstruction("ADD A, n", 2, 0xC6)]
        public static int ADD_A_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            AddBytes(cpu, cpu.Registers.A, data);

            return 7;
        }

        [MainInstruction("ADD A, (HL)", 1, 0x86)]
        public static int ADD_A_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            AddBytes(cpu, cpu.Registers.A, data);

            return 7;
        }

        [IXInstruction("ADD A, (IX+d)", 3, 0xDD, 0x86)]
        public static int ADD_A_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            AddBytes(cpu, cpu.Registers.A, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("ADD A, (IY+d)", 3, 0xFD, 0x86)]
        public static int ADD_A_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            AddBytes(cpu, cpu.Registers.A, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [MainInstruction("ADC A, A", 1, 0x8F)]
        [MainInstruction("ADC A, B", 1, 0x88)]
        [MainInstruction("ADC A, C", 1, 0x89)]
        [MainInstruction("ADC A, D", 1, 0x8A)]
        [MainInstruction("ADC A, E", 1, 0x8B)]
        [MainInstruction("ADC A, H", 1, 0x8C)]
        [MainInstruction("ADC A, L", 1, 0x8D)]
        public static int ADC_A_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            AddBytes(cpu, cpu.Registers.A, data, true);

            return 4;
        }

        [MainInstruction("ADC A, n", 2, 0xCE)]
        public static int ADC_A_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            AddBytes(cpu, cpu.Registers.A, data, true);

            return 7;
        }

        [MainInstruction("ADC A, (HL)", 1, 0x8E)]
        public static int ADC_A_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            AddBytes(cpu, cpu.Registers.A, data, true);

            return 7;
        }

        [IXInstruction("ADC A, (IX+d)", 3, 0xDD, 0x8E)]
        public static int ADC_A_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            AddBytes(cpu, cpu.Registers.A, data, true);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("ADC A, (IY+d)", 3, 0xFD, 0x8E)]
        public static int ADC_A_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            AddBytes(cpu, cpu.Registers.A, data, true);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }
        #endregion

        #region Subtraction
        [MainInstruction("SUB A, A", 1, 0x97)]
        [MainInstruction("SUB A, B", 1, 0x90)]
        [MainInstruction("SUB A, C", 1, 0x91)]
        [MainInstruction("SUB A, D", 1, 0x92)]
        [MainInstruction("SUB A, E", 1, 0x93)]
        [MainInstruction("SUB A, H", 1, 0x94)]
        [MainInstruction("SUB A, L", 1, 0x95)]
        public static int SUB_A_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            SubBytes(cpu, cpu.Registers.A, data);

            return 4;
        }

        [MainInstruction("SUB A, n", 2, 0xD6)]
        public static int SUB_A_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            SubBytes(cpu, cpu.Registers.A, data);

            return 7;
        }

        [MainInstruction("SUB A, (HL)", 1, 0x96)]
        public static int SUB_A_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            SubBytes(cpu, cpu.Registers.A, data);

            return 7;
        }

        [IXInstruction("SUB A, (IX+d)", 3, 0xDD, 0x96)]
        public static int SUB_A_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            SubBytes(cpu, cpu.Registers.A, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("SUB A, (IY+d)", 3, 0xFD, 0x96)]
        public static int SUB_A_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            SubBytes(cpu, cpu.Registers.A, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [MainInstruction("SBC A, A", 1, 0x9F)]
        [MainInstruction("SBC A, B", 1, 0x98)]
        [MainInstruction("SBC A, C", 1, 0x99)]
        [MainInstruction("SBC A, D", 1, 0x9A)]
        [MainInstruction("SBC A, E", 1, 0x9B)]
        [MainInstruction("SBC A, H", 1, 0x9C)]
        [MainInstruction("SBC A, L", 1, 0x9D)]
        public static int SBC_A_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            SubBytes(cpu, cpu.Registers.A, data, true);

            return 4;
        }

        [MainInstruction("SBC A, n", 2, 0xDE)]
        public static int SBC_A_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            SubBytes(cpu, cpu.Registers.A, data, true);

            return 7;
        }

        [MainInstruction("SBC A, (HL)", 1, 0x9E)]
        public static int SBC_A_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            SubBytes(cpu, cpu.Registers.A, data, true);

            return 7;
        }

        [IXInstruction("SBC A, (IX+d)", 3, 0xDD, 0x9E)]
        public static int SBC_A_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            SubBytes(cpu, cpu.Registers.A, data, true);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("SBC A, (IY+d)", 3, 0xFD, 0x9E)]
        public static int SBC_A_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            SubBytes(cpu, cpu.Registers.A, data, true);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }
        #endregion

        #region AND
        [MainInstruction("AND A", 1, 0xA7)]
        [MainInstruction("AND B", 1, 0xA0)]
        [MainInstruction("AND C", 1, 0xA1)]
        [MainInstruction("AND D", 1, 0xA2)]
        [MainInstruction("AND E", 1, 0xA3)]
        [MainInstruction("AND H", 1, 0xA4)]
        [MainInstruction("AND L", 1, 0xA5)]
        public static int AND_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            LogicalAndBytes(cpu, data);

            return 4;
        }

        [MainInstruction("AND n", 2, 0xE6)]
        public static int AND_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            LogicalAndBytes(cpu, data);

            return 7;
        }

        [MainInstruction("AND (HL)", 1, 0xA6)]
        public static int AND_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            LogicalAndBytes(cpu, data);

            return 7;
        }

        [IXInstruction("AND (IX+d)", 3, 0xDD, 0xA6)]
        public static int AND_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            LogicalAndBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("AND (IY+d)", 3, 0xFD, 0xA6)]
        public static int AND_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            LogicalAndBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }
        #endregion

        #region OR
        [MainInstruction("OR A", 1, 0xB7)]
        [MainInstruction("OR B", 1, 0xB0)]
        [MainInstruction("OR C", 1, 0xB1)]
        [MainInstruction("OR D", 1, 0xB2)]
        [MainInstruction("OR E", 1, 0xB3)]
        [MainInstruction("OR H", 1, 0xB4)]
        [MainInstruction("OR L", 1, 0xB5)]
        public static int OR_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            LogicalOrBytes(cpu, data);

            return 4;
        }

        [MainInstruction("OR n", 2, 0xF6)]
        public static int OR_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            LogicalOrBytes(cpu, data);

            return 7;
        }

        [MainInstruction("OR (HL)", 1, 0xB6)]
        public static int OR_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            LogicalOrBytes(cpu, data);

            return 7;
        }

        [IXInstruction("OR (IX+d)", 3, 0xDD, 0xB6)]
        public static int OR_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            LogicalOrBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("OR (IY+d)", 3, 0xFD, 0xB6)]
        public static int OR_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            LogicalOrBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }
        #endregion

        #region XOR
        [MainInstruction("XOR A", 1, 0xAF)]
        [MainInstruction("XOR B", 1, 0xA8)]
        [MainInstruction("XOR C", 1, 0xA9)]
        [MainInstruction("XOR D", 1, 0xAA)]
        [MainInstruction("XOR E", 1, 0xAB)]
        [MainInstruction("XOR H", 1, 0xAC)]
        [MainInstruction("XOR L", 1, 0xAD)]
        public static int XOR_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            LogicalXorBytes(cpu, data);

            return 4;
        }

        [MainInstruction("XOR n", 2, 0xEE)]
        public static int XOR_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            LogicalXorBytes(cpu, data);

            return 7;
        }

        [MainInstruction("XOR (HL)", 1, 0xAE)]
        public static int XOR_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            LogicalXorBytes(cpu, data);

            return 7;
        }

        [IXInstruction("XOR (IX+d)", 3, 0xDD, 0xAE)]
        public static int XOR_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            LogicalXorBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("XOR (IY+d)", 3, 0xFD, 0xAE)]
        public static int XOR_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            LogicalXorBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }
        #endregion

        #region CP
        [MainInstruction("CP A", 1, 0xBF)]
        [MainInstruction("CP B", 1, 0xB8)]
        [MainInstruction("CP C", 1, 0xB9)]
        [MainInstruction("CP D", 1, 0xBA)]
        [MainInstruction("CP E", 1, 0xBB)]
        [MainInstruction("CP H", 1, 0xBC)]
        [MainInstruction("CP L", 1, 0xBD)]
        public static int CP_r(IZ80CPU cpu, byte[] instruction)
        {
            var src = instruction[0].ExtractBits(0, 3);
            var data = ReadByteFromCpuRegister(cpu, src);
            CompareBytes(cpu, data);

            return 4;
        }

        [MainInstruction("CP n", 2, 0xFE)]
        public static int CP_n(IZ80CPU cpu, byte[] instruction)
        {
            var data = instruction[1];
            CompareBytes(cpu, data);

            return 7;
        }

        [MainInstruction("CP (HL)", 1, 0xBE)]
        public static int CP_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            var data = cpu.ReadMemory(cpu.Registers.HL);
            CompareBytes(cpu, data);

            return 7;
        }

        [IXInstruction("CP (IX+d)", 3, 0xDD, 0xBE)]
        public static int CP_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IX.CalculateIndex(instruction[2]));
            CompareBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }

        [IYInstruction("CP (IY+d)", 3, 0xFD, 0xBE)]
        public static int CP_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(2);
            var data = cpu.ReadMemory(cpu.Registers.IY.CalculateIndex(instruction[2]));
            CompareBytes(cpu, data);

            cpu.ControlLines.SystemClock.TickMultiple(3);
            return 19;
        }
        #endregion

        #region Increment/Decrement
        [MainInstruction("INC A", 1, 0x3C)]
        [MainInstruction("INC B", 1, 0x04)]
        [MainInstruction("INC C", 1, 0x0C)]
        [MainInstruction("INC D", 1, 0x14)]
        [MainInstruction("INC E", 1, 0x1C)]
        [MainInstruction("INC H", 1, 0x24)]
        [MainInstruction("INC L", 1, 0x2C)]
        public static int INC_r(IZ80CPU cpu, byte[] instruction)
        {
            var reg = instruction[0].ExtractBits(3, 3);
            byte val;
            switch (reg)
            {
                case 0b111: cpu.Registers.A++; val = cpu.Registers.A; break;
                case 0b000: cpu.Registers.B++; val = cpu.Registers.B; break;
                case 0b001: cpu.Registers.C++; val = cpu.Registers.C; break;
                case 0b010: cpu.Registers.D++; val = cpu.Registers.D; break;
                case 0b011: cpu.Registers.E++; val = cpu.Registers.E; break;
                case 0b100: cpu.Registers.H++; val = cpu.Registers.H; break;
                case 0b101: cpu.Registers.L++; val = cpu.Registers.L; break;
                default: throw new InvalidOperationException();
            }

            IncrementByteSetConditionBits(cpu, val);

            return 4;
        }

        [MainInstruction("INC (HL)", 1, 0x34)]
        public static int INC_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);
            data++;
            cpu.WriteMemory(cpu.Registers.HL, data);

            IncrementByteSetConditionBits(cpu, data);

            return 11;
        }

        [IXInstruction("INC (IX+d)", 3, 0xDD, 0x34)]
        public static int INC_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);
            data++;
            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);
            
            IncrementByteSetConditionBits(cpu, data);

            return 23;
        }

        [IXInstruction("INC (IY+d)", 3, 0xFD, 0x34)]
        public static int INC_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);
            data++;
            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            IncrementByteSetConditionBits(cpu, data);

            return 23;
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
            var reg = instruction[0].ExtractBits(3, 3);
            byte val;
            switch (reg)
            {
                case 0b111: cpu.Registers.A--; val = cpu.Registers.A; break;
                case 0b000: cpu.Registers.B--; val = cpu.Registers.B; break;
                case 0b001: cpu.Registers.C--; val = cpu.Registers.C; break;
                case 0b010: cpu.Registers.D--; val = cpu.Registers.D; break;
                case 0b011: cpu.Registers.E--; val = cpu.Registers.E; break;
                case 0b100: cpu.Registers.H--; val = cpu.Registers.H; break;
                case 0b101: cpu.Registers.L--; val = cpu.Registers.L; break;
                default: throw new InvalidOperationException();
            }
            
            DecrementByteSetConditionBits(cpu, val);

            return 4;
        }

        [MainInstruction("DEC (HL)", 1, 0x35)]
        public static int DEC_HL_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.Tick();
            var data = cpu.ReadMemory(cpu.Registers.HL);
            data--;
            cpu.WriteMemory(cpu.Registers.HL, data);

            DecrementByteSetConditionBits(cpu, data);

            return 11;
        }

        [IXInstruction("DEC (IX+d)", 3, 0xDD, 0x35)]
        public static int DEC_IX_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var addr = cpu.Registers.IX.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);
            data--;
            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            DecrementByteSetConditionBits(cpu, data);

            return 23;
        }

        [IXInstruction("DEC (IY+d)", 3, 0xFD, 0x35)]
        public static int DEC_IY_plus_d_mem(IZ80CPU cpu, byte[] instruction)
        {
            cpu.ControlLines.SystemClock.TickMultiple(5);

            var addr = cpu.Registers.IY.CalculateIndex(instruction[2]);
            var data = cpu.ReadMemory(addr);
            data--;
            cpu.ControlLines.SystemClock.Tick();
            cpu.WriteMemory(addr, data);

            DecrementByteSetConditionBits(cpu, data);

            return 23;
        }

        #endregion

        #region General Purpose

        [MainInstruction("DAA", 1, 0x27)]
        public static int DAA(IZ80CPU cpu, byte[] instruction)
        {
            var acc = cpu.Registers.A;
            byte correction = 0;

            if (cpu.Registers.HalfCarry || acc.GetLowerNibble() > 9)
            {
                correction += 0x06;
                cpu.Registers.HalfCarry = true;
            }
            else
            {
                cpu.Registers.HalfCarry = false;
            }

            if (cpu.Registers.Carry || acc.GetUpperNibble() > 9)
            {
                correction += 0x60;
                cpu.Registers.Carry = true;
            }
            else
            {
                cpu.Registers.Carry = false;
            }

            if (cpu.Registers.Subtract)
            {
                correction = correction.TwosComplement();
            }

            acc = (byte) (acc + correction);
            cpu.Registers.A = acc;
            cpu.Registers.Sign = acc.IsNegative();
            cpu.Registers.Zero = acc == 0;
            cpu.Registers.ParityOrOverflow = acc.GetNumBitsSet() % 2 == 0;

            return 4;
        }

        [MainInstruction("CPL", 1, 0x2F)]
        public static int CPL(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.A = (byte) ~cpu.Registers.A;
            cpu.Registers.HalfCarry = true;
            cpu.Registers.Subtract = true;

            return 4;
        }

        [ExtendedInstruction("NEG", 2, 0xED, 0x44)]
        public static int NEG(IZ80CPU cpu, byte[] instruction)
        {
            var orig = cpu.Registers.A;
            var res = orig.TwosComplement();
            cpu.Registers.A = res;
            cpu.Registers.Sign = res.IsNegative();
            cpu.Registers.Zero = res == 0;
            cpu.Registers.HalfCarry = ((byte) 0).WillHalfCarry(res);
            cpu.Registers.ParityOrOverflow = orig == 0x80;
            cpu.Registers.Subtract = true;
            cpu.Registers.Carry = orig != 0;

            return 4;
        }

        [MainInstruction("CCF", 1, 0x3F)]
        public static int CCF(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.HalfCarry = cpu.Registers.Carry;
            cpu.Registers.Subtract = false;
            cpu.Registers.Carry = !cpu.Registers.Carry;

            return 4;
        }

        [MainInstruction("SCF", 1, 0x37)]
        public static int SCF(IZ80CPU cpu, byte[] instruction)
        {
            cpu.Registers.HalfCarry = false;
            cpu.Registers.Subtract = false;
            cpu.Registers.Carry = true;

            return 4;
        }

        #endregion

        #endregion

        #region 16-bit operations

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

        #endregion

        #region 8-bit Condition bit helpers

        private static void AddBytes(IZ80CPU cpu, byte a, byte b, bool useCarryFlag = false)
        {
            var carryFlagValue = (byte) (useCarryFlag ? 1 : 0);
            AddMultipleBytesAndSetConditionBits(cpu, false, a, b, carryFlagValue);
        }
        private static void SubBytes(IZ80CPU cpu, byte a, byte b, bool useCarryFlag = false)
        {
            var carryFlagValue = ((byte)(useCarryFlag ? 1 : 0)).TwosComplement();
            b = b.TwosComplement();
            AddMultipleBytesAndSetConditionBits(cpu, true, a, b, carryFlagValue);
        }
        private static void AddMultipleBytesAndSetConditionBits(IZ80CPU cpu, bool subtract, byte a, byte b, byte c)
        {
            var sum = a + b + c;
            var noCarrySum = a ^ b ^ c;

            var carryInto = sum ^ noCarrySum;
            var halfCarry = carryInto & 0x10;
            var carry = carryInto & 0x100;
            var overflow = carryInto & 0x80;

            var result = (byte)sum;
            cpu.Registers.Sign = result.IsNegative();
            cpu.Registers.Zero = result == 0;
            cpu.Registers.HalfCarry = halfCarry != 0;
            cpu.Registers.ParityOrOverflow = overflow != 0;
            cpu.Registers.Subtract = subtract;
            cpu.Registers.Carry = carry != 0;
            cpu.Registers.A = result;
        }

        private static void LogicalAndBytes(IZ80CPU cpu, byte b)
        {
            var result = (byte)(cpu.Registers.A & b);
            LogicalOperationBytesSetConditionBits(cpu, result);
        }

        private static void LogicalOrBytes(IZ80CPU cpu, byte b)
        {
            var result = (byte)(cpu.Registers.A | b);
            LogicalOperationBytesSetConditionBits(cpu, result);
        }

        private static void LogicalXorBytes(IZ80CPU cpu, byte b)
        {
            var result = (byte)(cpu.Registers.A ^ b);
            LogicalOperationBytesSetConditionBits(cpu, result);
        }

        private static void LogicalOperationBytesSetConditionBits(IZ80CPU cpu, byte result)
        {
            cpu.Registers.Sign = result.IsNegative();
            cpu.Registers.Zero = result == 0;
            cpu.Registers.HalfCarry = true;
            cpu.Registers.ParityOrOverflow = result.GetNumBitsSet() % 2 == 0;
            cpu.Registers.Subtract = false;
            cpu.Registers.Carry = false;
            cpu.Registers.A = result;
        }

        private static void CompareBytes(IZ80CPU cpu, byte b)
        {
            var cmp = (byte) (cpu.Registers.A - b);

            cpu.Registers.Sign = cmp.IsNegative();
            cpu.Registers.Zero = cmp == 0;
            cpu.Registers.HalfCarry = cpu.Registers.A.WillHalfCarry(b);
            cpu.Registers.ParityOrOverflow = cpu.Registers.A.WillOverflow(b);
            cpu.Registers.Subtract = true;
            cpu.Registers.Carry = cpu.Registers.A.WillCarry(b);
        }

        private static void IncrementByteSetConditionBits(IZ80CPU cpu, byte val)
        {
            cpu.Registers.Sign = val.IsNegative();
            cpu.Registers.Zero = val == 0;
            cpu.Registers.HalfCarry = (val & 0b1111) == 0;
            cpu.Registers.ParityOrOverflow = val == 0x80;
            cpu.Registers.Subtract = false;
        }

        private static void DecrementByteSetConditionBits(IZ80CPU cpu, byte val)
        {
            cpu.Registers.Sign = val.IsNegative();
            cpu.Registers.Zero = val == 0;
            cpu.Registers.HalfCarry = (val & 0b1111) == 0b1111;
            cpu.Registers.ParityOrOverflow = val == 0x7f;
            cpu.Registers.Subtract = true;
        }
        #endregion
    }
}
