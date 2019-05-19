using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public interface IZ80CPU : IDevice, IClockedComponent
    {
        Z80RegisterFile Registers { get; }
        Z80CPULines ControlLines { get; }

        byte FetchOpcode();
        void InsertWaitMachineCycle(int numCycles);
        byte ReadMemory(ushort address, int waitBefore = 0, int waitAfter = 0);
        ushort ReadWord(ushort address);
        byte PopByte();
        void WriteMemory(ushort address, byte data, int waitBefore = 0, int waitAfter = 0);
        void WriteWord(ushort address, ushort data);
        void PushWord(ushort data, int waitBefore = 0);
        byte ReadFromPort(ushort address, int waitBefore = 0);
        void WriteToPort(ushort address, byte data);
    }
}
