using System;
using NesEmu.Core;

namespace NesEmu.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var console = new NintendoEntertainmentSystem();

            console.LoadCartridge("D:\\Dev\\NesEmulator\\TestRoms\\official.nes");
        }
    }
}
