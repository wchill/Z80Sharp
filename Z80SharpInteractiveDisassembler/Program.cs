using System;
using System.IO;
using Z80Sharp;

namespace Z80SharpInteractiveDisassembler
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            if (args.Length < 1)
            {
                Console.WriteLine("Enter file path: ");
                path = Console.ReadLine();
            }
            else
            {
                path = args[0];
            }

            var data = File.ReadAllBytes(path);
            var mem = new byte[65536];
            Array.Copy(data, 0, mem, 0x100, data.Length);
            var cpu = new Z80Disassembler(mem);

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                var cmdArgs = input.Split(' ');
                switch (cmdArgs[0].ToLower())
                {
                    case "d":
                    case "dis":
                    case "disassemble":
                    {
                        var addr = Convert.ToUInt16(cmdArgs[1], 16);
                        var output = cpu.Disassemble(addr);
                        foreach (var disassembledInstruction in output)
                        {
                            Console.WriteLine($"[0x{disassembledInstruction.Address:X4}]: {disassembledInstruction.Mnemonic}");
                        }

                        break;
                    }
                    case "x":
                    case "examine":
                    {
                        var addr = Convert.ToUInt16(cmdArgs[1], 16);
                        var len = 1;
                        if (cmdArgs.Length > 2)
                        {
                            len = Convert.ToUInt16(cmdArgs[2]);
                        }
                        for (var i = 0; i < len; i++)
                        {
                            Console.WriteLine($"[0x{addr + i:X2}]: {mem[addr + i]:X2}");
                        }
                        break;
                    }
                    default:
                        Console.WriteLine($"Unrecognized command: {cmdArgs[0]}");
                        break;
                }
            }
        }
    }
}
