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
        private Cartridge _cartridge;

        public NintendoEntertainmentSystem()
        {
            Processor = new CPU();
            CpuBus = new CPUBus(Processor);
        }

        public void Reset()
        {
            //ToDo
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