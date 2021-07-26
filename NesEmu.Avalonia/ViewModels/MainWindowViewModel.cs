using System;
using System.Collections.Generic;
using System.Text;
using NesEmu.Core;

namespace NesEmu.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NintendoEntertainmentSystem _nes;

        public MainWindowViewModel(NintendoEntertainmentSystem nes)
        {
            _nes = nes ?? throw new ArgumentNullException("nes");
        }

        public string Greeting => "Welcome to Avalonia w/ Dependency Injection!";
        public string Secondary => "Center Left";
    }
}
