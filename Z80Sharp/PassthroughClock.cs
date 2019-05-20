using System;
using System.Collections.Generic;

namespace Z80Sharp
{
    public class PassthroughClock : IClock
    {
        public long Ticks { get; private set; }
        public HashSet<IClockedComponent> AttachedDevices { get; }

        public PassthroughClock()
        {
            AttachedDevices = new HashSet<IClockedComponent>();
        }

        public void AttachClockableDevice(IClockedComponent device)
        {
            AttachedDevices.Add(device);
        }
        public void DetachClockableDevice(IClockedComponent device)
        {
            AttachedDevices.Remove(device);
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