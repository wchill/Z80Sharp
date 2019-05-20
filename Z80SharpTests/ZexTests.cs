using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Z80Sharp;

namespace Z80SharpTests
{
    public class ZexTests
    {
        [Fact]
        public void TestZEXDOC()
        {
            var system = new Z80System();
            LoadIntoMemory(system.Memory, "zexdoc.com");
            var cpu = system.Cpu;
            cpu.Registers.PC = 0x100;

            while (true)
            {
                cpu.ExecuteNextInstruction();
            }
        }

        [Fact]
        public void TestZEXALL()
        {
            var system = new Z80System();
            LoadIntoMemory(system.Memory, "zexall.com");
            var cpu = system.Cpu;
            cpu.Registers.PC = 0x100;

            while (true)
            {
                cpu.ExecuteNextInstruction();
            }
        }

        private static void LoadIntoMemory(IMemory memory, string filename)
        {
            var data = File.ReadAllBytes(filename);
            memory.LoadIntoMemory(0x100, data);
        }
    }
}
