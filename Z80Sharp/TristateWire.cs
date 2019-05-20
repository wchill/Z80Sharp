using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public enum TristateWireState
    {
        HighImpedance = 0,
        LogicLow = 1,
        LogicHigh = 2,
        PullDown = 3,
        PullUp = 4
    }

    // A wire is a 1-bit bus that supports either logic high, logic low, or high-impedance states.
    public class TristateWire : IBus<TristateWireState>
    {
        private class NullDevice : IDevice
        {

        }

        private static readonly NullDevice wireDevice = new NullDevice();

        private readonly List<IDevice> _attachedDevices;
        public IReadOnlyList<IDevice> AttachedDevices => _attachedDevices;

        public TristateWireState Value { get; private set; }

        private Dictionary<IDevice, TristateWireState> Values { get; }

        public TristateWire()
        {
            Values = new Dictionary<IDevice, TristateWireState>();
            _attachedDevices = new List<IDevice>();
            AttachDevice(wireDevice);
        }
        public TristateWire(TristateWireState state) : this()
        {
            WriteValue(wireDevice, state);
        }

        public void WriteValue(IDevice device, TristateWireState? newValue)
        {
            Values[device] = newValue ?? TristateWireState.HighImpedance;
            Value = CalculateNewState();
        }

        public void WriteValue(TristateWireState newValue)
        {
            WriteValue(wireDevice, newValue);
        }

        public void AttachDevice(IDevice device)
        {
            _attachedDevices.Add(device);
            Values.Add(device, TristateWireState.HighImpedance);
        }

        public void DetachDevice(IDevice device)
        {
            _attachedDevices.Remove(device);
            Values.Remove(device);
        }

        private TristateWireState CalculateNewState()
        {
            var counts = new int[5];
            foreach (var value in Values.Values)
            {
                counts[(int)value]++;
            }

            var logicLow = counts[(int)TristateWireState.LogicLow];
            var logicHigh = counts[(int)TristateWireState.LogicHigh];
            if (logicLow > 0 || logicHigh > 0)
            {
                return logicLow > logicHigh ? TristateWireState.LogicLow : TristateWireState.LogicHigh;
            }

            var pullDown = counts[(int)TristateWireState.PullDown];
            var pullUp = counts[(int)TristateWireState.PullUp];
            if (pullDown > 0 || pullUp > 0)
            {
                return pullDown > pullUp ? TristateWireState.LogicLow : TristateWireState.LogicHigh;
            }

            return TristateWireState.HighImpedance;
        }
    }
}
