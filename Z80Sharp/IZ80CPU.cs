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
        void RefreshMemory();
        byte ReadMemory(ushort address);
        ushort ReadWord(ushort address);
        byte PopByte();
        void WriteMemory(ushort address, byte data);
        void WriteWord(ushort address, ushort data);
        void PushWord(ushort data);
        byte ReadFromPort(ushort address);
        void WriteToPort(ushort address, byte data);
    }
}
