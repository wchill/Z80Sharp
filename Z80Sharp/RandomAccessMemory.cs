using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public class RandomAccessMemory : IMemory
    {
        private readonly byte[] _memory;
        public Span<byte> Memory => _memory;
        public int Size => _memory.Length;
        public MemoryLines Connections { get; }
        public ushort BeginAddress { get; }
        public ushort EndAddress { get; }

        private bool IsAddressed => (Connections.AddressBus.Value >= BeginAddress && Connections.AddressBus.Value <= EndAddress);
        private ushort CurrentAddress => (ushort) (Connections.AddressBus.Value - BeginAddress);

        private bool _isWaiting;

        public RandomAccessMemory(ushort beginAddress, int size, MemoryLines connections)
        {
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be greater than zero");
            }

            _memory = new byte[size];
            BeginAddress = beginAddress;
            EndAddress = (ushort)(beginAddress + _memory.Length - 1);
            Connections = connections;
            Connections.AttachMemory(this);
        }

        public RandomAccessMemory(ushort beginAddress, ReadOnlySpan<byte> memory, MemoryLines connections)
        {
            _memory = memory.ToArray();
            BeginAddress = beginAddress;
            EndAddress = (ushort)(beginAddress + _memory.Length - 1);
            Connections = connections;
            Connections.AttachMemory(this);
        }

        public void LoadIntoMemory(ushort address, ReadOnlySpan<byte> data)
        {
            data.CopyTo(_memory.AsSpan(address));
        }

        public void Tick()
        {
            if (Connections.MREQ.Value == TristateWireState.LogicHigh ||
                (Connections.RD.Value == TristateWireState.LogicHigh && Connections.WR.Value == TristateWireState.LogicHigh))
            {
                return;
            }

            if (!IsAddressed) return;

            if (!_isWaiting)
            {
                _isWaiting = true;
                Connections.WAIT.WriteValue(this, TristateWireState.LogicHigh);
                if (Connections.WR.Value == TristateWireState.LogicLow)
                {
                    _memory[CurrentAddress] = Connections.DataBus.Value;
                }
                else if (Connections.RD.Value == TristateWireState.LogicLow)
                {
                    Connections.DataBus.WriteValue(this, _memory[CurrentAddress]);
                }
            }
            else
            {
                _isWaiting = false;
                Connections.WAIT.WriteValue(this, TristateWireState.HighImpedance);
            }
        }
    }
}
