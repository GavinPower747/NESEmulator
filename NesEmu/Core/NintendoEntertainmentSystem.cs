using NesEmu.Devices;
using NesEmu.Devices.CPU;
using NesEmu.Devices.Cartridge;

namespace NesEmu.Core
{

    ///<summary>
    ///The main public API to access console functions
    ///</summary>
    public class NintendoEntertainmentSystem
    {
        public readonly CPU Processor;
        public readonly CPUBus CpuBus;
        private readonly Ram _ram;
        private Cartridge _cartridge;

        public NintendoEntertainmentSystem()
        {
            Processor = new CPU();
            CpuBus = new CPUBus(Processor);
            _ram = new Ram();

            CpuBus.ConnectDevice(_ram);
        }

        public void Reset()
        {
            Processor.Reset();
        }

        public void Interrupt()
        {
            Processor.Interrupt();
        }

        public void NonMaskableInterrupt()
        {
            Processor.NonMaskableInterrupt();
        }

        public void LoadCartridge(string pathToRom)
        {
            _cartridge = new Cartridge(pathToRom);
            _cartridge.Load();

            //For now write the contents of the cart to $8000 and beyond.
            //This lets us test the CPU in isolation with a test cart
            for(var i = 0; i < _cartridge._programBody.Count; i++)
            {
                var programByte = _cartridge._programBody[i];
                _ram.Write((ushort)(0x8000 + i), programByte);
            }
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