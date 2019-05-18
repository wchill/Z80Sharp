using System;
using System.Collections.Generic;
using System.Text;

namespace Z80Sharp
{
    public class Z80System
    {
        private readonly Z80CPU _cpu;
        private readonly AddressBus _addressBus;
        private readonly DataBus _dataBus;
        private readonly Z80CPULines _cpuLines;
        private readonly RandomAccessMemory _z80Ram;
        private readonly IClock _z80Clock;

        public Z80System()
        {
            _dataBus = new DataBus();
            _addressBus = new AddressBus();
            _z80Clock = new PassthroughClock();
            _cpuLines = new Z80CPULines
            {
                AddressBus = _addressBus,
                DataBus = _dataBus,
                SystemClock = _z80Clock,
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
                AddressBus = _addressBus,
                Clock = _z80Clock,
                DataBus = _dataBus,
                MREQ = _cpuLines.MREQ,
                RD = _cpuLines.RD,
                WAIT = _cpuLines.WAIT,
                WR = _cpuLines.WR
            };
            _z80Ram = new RandomAccessMemory(0, 0x4000, memoryConnects);
            _cpu = new Z80CPU(_cpuLines);
        }
    }
}
