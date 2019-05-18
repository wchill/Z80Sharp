using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public class DataBus : IBus<byte>
    {
        // TODO: When no device is writing to the bus, the bus should be reset to a default value.
        private readonly List<IDevice> _attachedDevices;
        public IReadOnlyList<IDevice> AttachedDevices => _attachedDevices;
        public byte Value { get; private set; }

        public DataBus()
        {
            _attachedDevices = new List<IDevice>();
        }

        public void WriteValue(IDevice device, byte value)
        {
            Value = value;
        }

        public void AttachDevice(IDevice device)
        {
            _attachedDevices.Add(device);
        }
    }
}
