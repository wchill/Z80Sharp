using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public class Z80System
    {
        public readonly Z80CPU Cpu;
        public readonly AddressBus AddressBus;
        public readonly DataBus DataBus;
        public readonly Z80CPULines CpuLines;
        public readonly RandomAccessMemory Memory;
        public readonly IClock Z80Clock;

        public Z80System()
        {
            DataBus = new DataBus();
            AddressBus = new AddressBus();
            Z80Clock = new PassthroughClock();
            CpuLines = new Z80CPULines
            {
                AddressBus = AddressBus,
                DataBus = DataBus,
                SystemClock = Z80Clock,
                BUSACK = new TristateWire(),
                BUSREQ = new TristateWire(TristateWireState.PullUp),
                HALT = new TristateWire(),
                INT = new TristateWire(TristateWireState.PullUp),
                IORQ = new TristateWire(),
                M1 = new TristateWire(),
                MREQ = new TristateWire(),
                NMI = new TristateWire(TristateWireState.PullUp),
                RD = new TristateWire(),
                RESET = new TristateWire(TristateWireState.PullUp),
                RFSH = new TristateWire(),
                WAIT = new TristateWire(TristateWireState.PullUp),
                WR = new TristateWire()
            };

            var memoryConnects = new MemoryLines
            {
                AddressBus = AddressBus,
                Clock = Z80Clock,
                DataBus = DataBus,
                MREQ = CpuLines.MREQ,
                RD = CpuLines.RD,
                WAIT = CpuLines.WAIT,
                WR = CpuLines.WR
            };
            Memory = new RandomAccessMemory(0, 0x10000, memoryConnects);
            Cpu = new Z80CPU(CpuLines);
        }
    }
}
