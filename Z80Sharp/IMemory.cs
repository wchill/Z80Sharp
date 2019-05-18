using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public interface IMemory : IIODevice
    {
        int Size { get; }
        MemoryLines Connections { get; }
    }
}
