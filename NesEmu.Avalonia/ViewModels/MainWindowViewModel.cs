using System;
using System.Collections.Generic;
using System.Text;
using NesEmu.Core;
using NesEmu.Devices.CPU;

namespace NesEmu.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NintendoEntertainmentSystem _nes;

        private CPURegisters _cpuRegisters;

        public MainWindowViewModel(NintendoEntertainmentSystem nes)
        {
            _nes = nes ?? throw new ArgumentNullException("nes");
            _cpuRegisters = _nes.Processor.Registers;
        }
    }
}
