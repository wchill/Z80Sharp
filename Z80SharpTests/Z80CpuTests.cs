using System.Linq;
using Xunit;
using Z80Sharp;
using Z80Sharp.Instructions;

namespace Z80SharpTests
{
    public class Z80CpuTests
    {
        [Fact]
        public void TestOpcodeFetchTakes4Cycles()
        {
            var cpu = GetZ80Cpu();
            Assert.Equal(0, cpu.Registers.R);

            var clock = cpu.ControlLines.SystemClock;
            cpu.FetchOpcode();

            Assert.Equal(4, clock.Ticks);
            Assert.Equal(1, cpu.Registers.R);
        }

        [Fact]
        public void TestMemoryReadTakes3Cycles()
        {
            var cpu = GetZ80Cpu();
            var clock = cpu.ControlLines.SystemClock;
            cpu.ReadMemory(0);

            Assert.Equal(3, clock.Ticks);
        }

        [Fact]
        public void TestMemoryWriteTakes3Cycles()
        {
            var cpu = GetZ80Cpu();
            var clock = cpu.ControlLines.SystemClock;
            cpu.WriteMemory(0, 0b1);

            Assert.Equal(3, clock.Ticks);
        }

        [Fact]
        public void TestPortReadTakes4Cycles()
        {
            var cpu = GetZ80Cpu();
            var clock = cpu.ControlLines.SystemClock;
            cpu.ReadFromPort(0);

            Assert.Equal(4, clock.Ticks);
        }

        [Fact]
        public void TestPortWriteTakes4Cycles()
        {
            var cpu = GetZ80Cpu();
            var clock = cpu.ControlLines.SystemClock;
            cpu.WriteToPort(0, 0b1);

            Assert.Equal(4, clock.Ticks);
        }

        [Fact]
        public void TestMemoryWriteThenRead()
        {
            var cpu = GetZ80Cpu();
            cpu.WriteMemory(0, 0b1);

            var data = cpu.ReadMemory(0);
            Assert.Equal(1, data);

            data = cpu.FetchOpcode();
            Assert.Equal(1, data);
        }

        private static Z80CPU GetZ80Cpu()
        {
            var dataBus = new DataBus();
            var addressBus = new AddressBus();
            var z80Clock = new PassthroughClock();
            var cpuLines = new Z80CPULines
            {
                AddressBus = addressBus,
                DataBus = dataBus,
                SystemClock = z80Clock,
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
                AddressBus = addressBus,
                Clock = z80Clock,
                DataBus = dataBus,
                MREQ = cpuLines.MREQ,
                RD = cpuLines.RD,
                WAIT = cpuLines.WAIT,
                WR = cpuLines.WR
            };
            var unused = new RandomAccessMemory(0, 0x4000, memoryConnects);
            return new Z80CPU(cpuLines);
        }
    }
}