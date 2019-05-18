using System;
using Moq;
using Xunit;
using Z80Sharp;

namespace Z80SharpTests
{
    public class RandomAccessMemoryTests
    {
        [Fact]
        public void TestReadFromRam()
        {
            var random = new Random();
            var lines = GetMemoryLines();

            var data = new byte[0x100];
            random.NextBytes(data);

            var unused = new RandomAccessMemory(0x100, data, lines);
            var mockDevice = new Mock<IDevice>();

            for (var i = 0; i < data.Length; i++)
            {
                lines.AddressBus.WriteValue(mockDevice.Object, (ushort)(0x100 + i));
                lines.MREQ.WriteValue(TristateWireState.LogicLow);
                lines.RD.WriteValue(TristateWireState.LogicLow);
                lines.WAIT.WriteValue(TristateWireState.HighImpedance);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.LogicHigh, lines.WAIT.Value);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.HighImpedance, lines.WAIT.Value);
                Assert.Equal(data[i], lines.DataBus.Value);

                lines.MREQ.WriteValue(TristateWireState.LogicHigh);
                lines.RD.WriteValue(TristateWireState.LogicHigh);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.HighImpedance, lines.WAIT.Value);
                Assert.Equal(data[i], lines.DataBus.Value);
            }
        }

        [Fact]
        public void TestReadFromRamOtherAddresses()
        {
            var random = new Random();
            var lines = GetMemoryLines();

            var data = new byte[0x100];
            random.NextBytes(data);

            var unused = new RandomAccessMemory(0x100, data, lines);
            var mockDevice = new Mock<IDevice>();

            var randomValue = (byte)(random.Next() % 256);
            lines.DataBus.WriteValue(mockDevice.Object, randomValue);

            for (var i = 0; i < data.Length; i++)
            {
                lines.AddressBus.WriteValue(mockDevice.Object, (ushort)(0x100 + data.Length + i));
                lines.MREQ.WriteValue(TristateWireState.LogicLow);
                lines.RD.WriteValue(TristateWireState.LogicLow);
                lines.WAIT.WriteValue(TristateWireState.HighImpedance);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.HighImpedance, lines.WAIT.Value);
                Assert.Equal(randomValue, lines.DataBus.Value);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.HighImpedance, lines.WAIT.Value);
                Assert.Equal(randomValue, lines.DataBus.Value);

                lines.MREQ.WriteValue(TristateWireState.LogicHigh);
                lines.RD.WriteValue(TristateWireState.LogicHigh);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.HighImpedance, lines.WAIT.Value);
                Assert.Equal(randomValue, lines.DataBus.Value);
            }
        }

        [Fact]
        public void TestWriteToRamChange()
        {
            var random = new Random();
            var lines = GetMemoryLines();

            var data = new byte[0x100];
            random.NextBytes(data);

            var memory = new RandomAccessMemory(0x100, data, lines);
            var mockDevice = new Mock<IDevice>();

            for (var i = 0; i < data.Length; i++)
            {
                var randomValue = (byte) (random.Next() % 256);
                lines.AddressBus.WriteValue(mockDevice.Object, (ushort)(0x100 + i));
                lines.DataBus.WriteValue(mockDevice.Object, randomValue);
                lines.MREQ.WriteValue(TristateWireState.LogicLow);
                lines.WR.WriteValue(TristateWireState.LogicLow);
                lines.WAIT.WriteValue(TristateWireState.HighImpedance);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.LogicHigh, lines.WAIT.Value);
                Assert.Equal(randomValue, memory.Memory[i]);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.HighImpedance, lines.WAIT.Value);
                Assert.Equal(randomValue, memory.Memory[i]);

                lines.MREQ.WriteValue(TristateWireState.LogicHigh);
                lines.WR.WriteValue(TristateWireState.LogicHigh);

                lines.Clock.Tick();
                Assert.Equal(TristateWireState.HighImpedance, lines.WAIT.Value);
                Assert.Equal(randomValue, memory.Memory[i]);
            }
        }

        private static MemoryLines GetMemoryLines()
        {
            var lines = new MemoryLines
            {
                AddressBus = new AddressBus(),
                DataBus = new DataBus(),
                Clock = new PassthroughClock(),
                MREQ = new TristateWire(),
                RD = new TristateWire(),
                WAIT = new TristateWire(),
                WR = new TristateWire()
            };
            return lines;
        }
    }
}