using NesEmu.Devices;
using NesEmu.Devices.CPU;
using NesEmu.Devices.Cartridge;
using NesEmu.Devices.PPU;

namespace NesEmu.Core
{

    ///<summary>
    ///The main public API to access console functions
    ///</summary>
    public class NintendoEntertainmentSystem
    {
        public readonly Disassembler Disassembler;
        public readonly PPU PPU;
        private readonly CPU _processor;
        private readonly CPUBus _cpuBus;
        private readonly Ram _ram;
        private Cartridge _cartridge;

        public NintendoEntertainmentSystem()
        {
            _processor = new CPU();
            _cpuBus = new CPUBus(_processor);
            PPU = new PPU();
            Disassembler = new Disassembler(_cpuBus, _processor);
            _ram = new Ram();

            _cpuBus.ConnectDevice(_ram);
        }

        public void Reset()
        {
            _processor.Reset();
        }

        public void Interrupt()
        {
            _processor.Interrupt();
        }

        public void NonMaskableInterrupt()
        {
            _processor.NonMaskableInterrupt();
        }

        public void LoadCartridge(string pathToRom)
        {
            _cartridge = new Cartridge(pathToRom);
            _cartridge.Load();
        }

        public void SaveState()
        {
            //TODO
        }

        public void LoadState()
        {
            //Todo
        }
    }
}