using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public class DataBus : IBus<byte>
    {
        private readonly List<IDevice> _attachedDevices;
        public IReadOnlyList<IDevice> AttachedDevices => _attachedDevices;
        public byte Value => _value.GetValueOrDefault(0xFF);
        private byte? _value;

        public DataBus()
        {
            _attachedDevices = new List<IDevice>();
        }

        public void WriteValue(IDevice device, byte? value)
        {
            _value = value;
        }

        public void AttachDevice(IDevice device)
        {
            _attachedDevices.Add(device);
        }
    }
}
