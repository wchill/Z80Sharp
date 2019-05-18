using System;

namespace Z80Sharp
{
    public partial class Z80CPU : IZ80CPU
    {
        public Z80RegisterFile Registers { get; }
        public Z80CPULines ControlLines { get; }

        public Z80CPU(Z80CPULines lines)
        {
            Registers = new Z80RegisterFile();
            ControlLines = lines;

            lines.AttachCpu(this);
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }

        public byte FetchOpcode()
        {
            ControlLines.AddressBus.WriteValue(this, Registers.PC);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.M1.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            var opcode = ControlLines.DataBus.Value;

            // Memory refresh
            Registers.R++;
            var refreshAddr = Utilities.SetUpperByteOfWord((ushort) (Registers.R & 0x7F), Registers.I);

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.AddressBus.WriteValue(this, refreshAddr);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);

            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.M1.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RFSH.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            return opcode;
        }

        public byte ReadMemory(ushort address)
        {
            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();
            return ControlLines.DataBus.Value;
        }

        public void WriteMemory(ushort address, byte data)
        {
            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.DataBus.WriteValue(this, data);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            ControlLines.MREQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();
        }

        public byte ReadFromPort(ushort address)
        {
            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            var data = ControlLines.DataBus.Value;

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.RD.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            return data;
        }

        public void WriteToPort(ushort address, byte data)
        {
            ControlLines.AddressBus.WriteValue(this, address);
            ControlLines.DataBus.WriteValue(this, data);
            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicLow);
            ControlLines.SystemClock.Tick();

            WaitForMemory();

            ControlLines.IORQ.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.WR.WriteValue(this, TristateWireState.LogicHigh);
            ControlLines.SystemClock.Tick();
        }

        private void WaitForMemory()
        {
            // Note that this is technically incorrect, the WAIT line is sampled halfway
            // through a clock cycle. However, our emulated memory should be asserting WAIT
            // for the entire clock cycle that the actual write occurs, so it should be fine.

            // Also note that we use a do/while loop because we always have to wait at least 1
            // cycle even if the WAIT line is brought HIGH immediately.
            do
            {
                ControlLines.SystemClock.Tick();
            } while (ControlLines.WAIT.Value != TristateWireState.LogicHigh);
        }
    }
}
