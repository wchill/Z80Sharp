using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public class AddressBus : IBus<ushort>
    {
        private readonly List<IDevice> _attachedDevices;
        public IReadOnlyList<IDevice> AttachedDevices => _attachedDevices;

        public ushort Value => _value.GetValueOrDefault(0xFFFF);

        private ushort? _value;

        public AddressBus()
        {
            _attachedDevices = new List<IDevice>();
        }

        public void WriteValue(IDevice device, ushort value)
        {
            _value = value;
        }

        public void AttachDevice(IDevice device)
        {
            _attachedDevices.Add(device);
        }
    }
}
