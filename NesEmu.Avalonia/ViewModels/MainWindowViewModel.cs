using System.Reflection.PortableExecutable;
using System.Collections.ObjectModel;
using Avalonia.Media;
using System;
using NesEmu.Core;
using NesEmu.Devices.CPU;
using ReactiveUI;
using System.Text;
using System.Collections.Generic;

namespace NesEmu.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IBrush CarryColour
        {
            get => _carryColour;
            set => this.RaiseAndSetIfChanged(ref _carryColour, value);
        }

        public IBrush ZeroColour
        {
            get => _zeroColour;
            set => this.RaiseAndSetIfChanged(ref _zeroColour, value);
        }

        public IBrush InterruptColour
        {
            get => _interruptColour;
            set => this.RaiseAndSetIfChanged(ref _interruptColour, value);
        }

        public IBrush DecimalColour
        {
            get => _decimalColour;
            set => this.RaiseAndSetIfChanged(ref _decimalColour, value);
        }

        public IBrush BreakColour
        {
            get =>_breakColour;
            set => this.RaiseAndSetIfChanged(ref _breakColour, value);
        }

        public IBrush OverflowColour
        {
            get => _overflowColour;
            set => this.RaiseAndSetIfChanged(ref _overflowColour, value);
        }

        public IBrush NegativeColour
        {
            get =>_negativeColour;
            set => this.RaiseAndSetIfChanged(ref _negativeColour, value);
        }

        public string ProgramCounter
        {
            get => _programCounter;
            set => this.RaiseAndSetIfChanged(ref _programCounter, value);
        }

        public string Accumulator
        {
            get => _accumulator;
            set => this.RaiseAndSetIfChanged(ref _accumulator, value);
        }

        public string StackPointer
        {
            get => _stackPointer;
            set => this.RaiseAndSetIfChanged(ref _stackPointer, value);
        }

        public string XRegister
        {
            get => _xRegister;
            set => this.RaiseAndSetIfChanged(ref _xRegister, value);
        }

        public string YRegister
        {
            get => _yRegister;
            set => this.RaiseAndSetIfChanged(ref _yRegister, value);
        }

        public string MemoryString
        {
            get => _memoryString;
            set => this.RaiseAndSetIfChanged(ref _memoryString, value);
        }

        public List<(ushort key, string value, bool isActive, string displayValue)> DisassembledCode
        {
            get => _disassembledCode;
            set => this.RaiseAndSetIfChanged(ref _disassembledCode, value);
        }

        private readonly NintendoEntertainmentSystem _nes;

        private CPURegisters _cpuRegisters;

        private IBrush _carryColour;
        private IBrush _zeroColour;
        private IBrush _interruptColour;
        private IBrush _decimalColour;
        private IBrush _breakColour;
        private IBrush _overflowColour;
        private IBrush _negativeColour;
        private string _programCounter;
        private string _accumulator;
        private string _stackPointer;
        private string _xRegister;
        private string _yRegister;
        private string _memoryString;
        private List<(ushort key, string value, bool isActive, string displayValue)> _disassembledCode;

        public MainWindowViewModel(NintendoEntertainmentSystem nes)
        {
            _nes = nes ?? throw new ArgumentNullException("nes");
            _cpuRegisters = _nes.Processor.Registers;

            //Declare them all here in the 
            //constructor so the compiler stops talking shit
            _carryColour = Brushes.Red;
            _zeroColour = Brushes.Red;
            _interruptColour = Brushes.Red;
            _decimalColour = Brushes.Red;
            _breakColour = Brushes.Red;
            _overflowColour = Brushes.Red;
            _negativeColour = Brushes.Red;
            _programCounter = string.Empty;
            _accumulator = string.Empty;
            _stackPointer = string.Empty;
            _xRegister = string.Empty;
            _yRegister = string.Empty;
            _memoryString = string.Empty;
            _disassembledCode = new List<(ushort, string value, bool isActive, string displayValue)>();

            LoadExampleProgram();
            UpdateMemory();
            UpdateRegisters();
        }

        public void StepCPU()
        {
            _nes.Processor.Tick();

            UpdateMemory();
            UpdateRegisters();
        }

        private void UpdateMemory()
        {
            var builder = new StringBuilder();

            for(int i = 0; i < 64 * 1024; i += 2)
            {
                var lo = _nes.CpuBus.ReadByte((ushort)i);
                var hi = _nes.CpuBus.ReadByte((ushort)(i+1));

                builder.Append(BitConverter.ToString(new byte[2]{lo, hi}) + "    ");
            }

            MemoryString = builder.ToString();
        }
        
        private void UpdateRegisters()
        {
            _cpuRegisters = _nes.Processor.Registers;

            CarryColour = _cpuRegisters.StatusRegister.Carry ? Brushes.Green : Brushes.Red;
            ZeroColour = _cpuRegisters.StatusRegister.Zero ? Brushes.Green : Brushes.Red;
            InterruptColour = _cpuRegisters.StatusRegister.InterruptDisable ? Brushes.Green : Brushes.Red;
            DecimalColour = _cpuRegisters.StatusRegister.Decimal ? Brushes.Green : Brushes.Red;
            BreakColour = _cpuRegisters.StatusRegister.Break ? Brushes.Green : Brushes.Red;
            OverflowColour = _cpuRegisters.StatusRegister.Overflow ? Brushes.Green : Brushes.Red;
            NegativeColour = _cpuRegisters.StatusRegister.Negative ? Brushes.Green : Brushes.Red;

            Accumulator = "Accumulator: " + _cpuRegisters.Accumulator.ToString("X2");
            StackPointer = "Stack Pointer: " + _cpuRegisters.StackPointer.ToString("X2");
            ProgramCounter = "Program Counter: " + _cpuRegisters.ProgramCounter.ToString("X2");
            XRegister = "X Register: " + _cpuRegisters.X.ToString("X2");
            YRegister = "Y Register: " + _cpuRegisters.Y.ToString("X2");

            //It's 3am fuck off with your judgement
            var newList = new List<(ushort key, string value, bool isActive, string displayValue)>();
            foreach(var code in DisassembledCode)
            {
                (ushort key, string value, bool isActive, string displayValue) newCode;

                if(code.key == _cpuRegisters.ProgramCounter)
                    newCode = (code.key, code.value, true, code.key.ToString("X2"));
                else
                    newCode = (code.key, code.value, false, code.key.ToString("X2"));

                newList.Add(newCode);
            }

            DisassembledCode = newList;
        }

        private void LoadExampleProgram()
        {
            /*
                *=$8000
                LDX #10
                STX $0000
                LDX #3
                STX $0001
                LDY $0000
                LDA #0
                CLC
                loop
                ADC $0001
                DEY
                BNE loop
                STA $0002
                NOP
                NOP
                NOP
		    */
            byte[] program = new byte[] {0xA2, 0x0A, 0x8E, 0x00, 0x00, 0xA2, 0x03, 0x8E, 0x01, 0x00, 0xAC, 0x00, 0x00, 0xA9, 0x00, 0x18, 0x6D, 0x01, 0x00, 0x88, 0xD0, 0xFA, 0x8D, 0x02, 0x00, 0xEA, 0xEA, 0xEA};
            ushort offset = 0x8000;

            foreach(var b in program)
            {
                _nes.CpuBus.Write(offset++, b);
            }

            _nes.CpuBus.Write(0xFFFC, 0x00);
		    _nes.CpuBus.Write(0xFFFD, 0x80);

            _nes.Processor.Reset();

            var disassembledValues = _nes.Processor.GetDisassembly(0x0000, 0xFFFF);
            
            foreach(var disassembledValue in disassembledValues)
            {
                DisassembledCode.Add((disassembledValue.Key, disassembledValue.Value, disassembledValue.Key == _cpuRegisters.ProgramCounter, disassembledValue.Key.ToString("X2")));
            }
        }
    }
}
