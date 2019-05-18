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
        byte ReadMemory(ushort address);
        void WriteMemory(ushort address, byte data);
        byte ReadFromPort(ushort address);
        void WriteToPort(ushort address, byte data);
    }
}
