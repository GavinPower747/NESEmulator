using System;
using NesEmu.Core;

namespace NesEmu.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Running...");
            var console = new NintendoEntertainmentSystem();

            console.LoadCartridge("D:\\Dev\\NesEmulator\\TestRoms\\nestest.nes");

            console.Disassembler.GetCPUDisassembly(0x0000, 0xFFFF);
        }
    }
}
