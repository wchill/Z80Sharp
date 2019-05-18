using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public interface IIODevice : IDevice, IClockedComponent
    {
        ushort BeginAddress { get; }
        ushort EndAddress { get; }
    }
}
