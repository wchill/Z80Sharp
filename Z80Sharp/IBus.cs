using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public interface IBus<T> where T : struct
    {
        IReadOnlyList<IDevice> AttachedDevices { get; }
        T Value { get; }
        void WriteValue(IDevice device, T? value);
        void AttachDevice(IDevice device);
    }
}
