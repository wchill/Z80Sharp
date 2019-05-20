namespace Z80Sharp
{
    public class Z80CPULines
    {
        public TristateWire BUSACK;
        public TristateWire BUSREQ;
        public TristateWire HALT;
        public TristateWire INT;
        public TristateWire IORQ;
        public TristateWire M1;
        public TristateWire MREQ;
        public TristateWire NMI;
        public TristateWire RD;
        public TristateWire RESET;
        public TristateWire RFSH;
        public TristateWire WAIT;
        public TristateWire WR;
        public IBus<ushort> AddressBus;
        public IBus<byte> DataBus;
        public IClock SystemClock;

        public void AttachCpu(IZ80CPU cpu)
        {
            AddressBus.AttachDevice(cpu);
            DataBus.AttachDevice(cpu);
            SystemClock.AttachClockableDevice(cpu);
            BUSACK.AttachDevice(cpu);
            BUSREQ.AttachDevice(cpu);
            HALT.AttachDevice(cpu);
            INT.AttachDevice(cpu);
            IORQ.AttachDevice(cpu);
            M1.AttachDevice(cpu);
            MREQ.AttachDevice(cpu);
            NMI.AttachDevice(cpu);
            RD.AttachDevice(cpu);
            RESET.AttachDevice(cpu);
            RFSH.AttachDevice(cpu);
            WAIT.AttachDevice(cpu);
            WR.AttachDevice(cpu);
        }
    }
}