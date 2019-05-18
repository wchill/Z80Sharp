namespace Z80Sharp
{
    public class MemoryLines
    {
        public IClock Clock;
        public IBus<ushort> AddressBus;
        public IBus<byte> DataBus;
        public TristateWire MREQ;
        public TristateWire RD;
        public TristateWire WR;
        public TristateWire WAIT;

        public void AttachMemory(IMemory memory)
        {
            AddressBus.AttachDevice(memory);
            DataBus.AttachDevice(memory);
            Clock.AttachClockableDevice(memory);
            MREQ.AttachDevice(memory);
            RD.AttachDevice(memory);
            WR.AttachDevice(memory);
            WAIT.AttachDevice(memory);
        }
    }
}