using NesEmu.Devices.CPU;
using NesEmu.Devices.Cartridge;

namespace NesEmu.Core
{

    ///<summary>
    ///The main public API to access console functions
    ///</summary>
    public class NintendoEntertainmentSystem
    {
        private readonly CPU _processor;
        private readonly CPUBus _cpuBus;
        private Cartridge _cartridge;

        public NintendoEntertainmentSystem()
        {
            _processor = new CPU();
            _cpuBus = new CPUBus(_processor);
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