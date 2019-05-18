using System;
using System.Collections.Generic;

namespace Z80Sharp
{
    public class PassthroughClock : IClock
    {
        public long Ticks { get; private set; }
        public List<IClockedComponent> AttachedDevices { get; }

        public PassthroughClock()
        {
            AttachedDevices = new List<IClockedComponent>();
        }

        public void AttachClockableDevice(IClockedComponent device)
        {
            AttachedDevices.Add(device);
        }

        public void TickMultiple(int ticks)
        {
            for (var i = 0; i < ticks; i++)
            {
                Tick();
            }
        }

        public void Tick()
        {
            foreach (var device in AttachedDevices)
            {
                device.Tick();
            }
            Ticks += 1;
        }
    }
}