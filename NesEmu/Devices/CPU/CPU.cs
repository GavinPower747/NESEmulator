using System;
using System.Collections.Generic;
using System.Linq;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Devices.CPU.Instructions.Operations;

namespace NesEmu.Devices.CPU
{
    public class CPU : IClockAware
    {
        public CPURegisters Registers;

        private IBus _bus;
        private int _cycles = 0;
        private Dictionary<ushort, Instruction> _opcodeLookup;
        private readonly Instruction _noOpInstruction = new Instruction("NOP", new ImpliedAddressing(), new NoOpOperation(), 2);

        public CPU()
        {
            Registers = new CPURegisters();

            _opcodeLookup = new Dictionary<ushort, Instruction>();

            PopulateOpcodeLookup();
        }

        public void ConnectBus(IBus bus) => _bus = bus;

        public void Tick()
        {
            //The traditional NES does operations in multiple cycles, there is no need for us to do it
            //like that just do everything on the last cycle
            if(_cycles == 0)
            {
                var opcode = _bus.ReadByte(Registers.ProgramCounter);
                Instruction instruction = null;

                _opcodeLookup.TryGetValue(opcode, out instruction);

                if(instruction is null)
                    instruction = _noOpInstruction;

                _cycles = instruction.Cycles;
                Registers.ProgramCounter++;

                var addressInfo = instruction.AddressingStrategy.GetOperationAddress(Registers, _bus);
                var operationExtraCycles = instruction.OperationStrategy.Operate(addressInfo.address, Registers, _bus);

                _cycles += addressInfo.extraCycles + operationExtraCycles;
            }

            _cycles--;
        }

        public void Reset()
        {
            ushort address = 0xFFFC;
            Registers.ProgramCounter = _bus.ReadWord(address);

            Registers.Accumulator = 0;
            Registers.X = 0;
            Registers.Y = 0;
            Registers.StackPointer = 0xFD;
            Registers.StatusRegister = new StatusRegister(0x00);

            _cycles = 8;
        }

        public bool OpComplete() => _cycles == 0;

        public void Interrupt()
        {
            if(Registers.StatusRegister.InterruptDisable)
                return;

            PerformInterrupt();
        }

        public void NonMaskableInterrupt()
        {
            PerformInterrupt();
        }

        public Dictionary<ushort, string> GetDisassembly(ushort start, ushort end)
        {
            var values = new Dictionary<ushort, string>();
            var address = start;

            while(address < end)
            {
                Instruction instruction = null;
                var opcode = _bus.ReadByte(address);
                var lineAddress = address;
                var lineString = string.Empty;

                if(opcode == 0)
                {
                    address++;
                    continue;
                }

                _opcodeLookup.TryGetValue(opcode, out instruction);

                if(instruction is null)
                    instruction = _noOpInstruction;

                address++;

                lineString = instruction.Name + "  ";

                switch (instruction.AddressingStrategy)
                {
                    case ImpliedAddressing _:
                        lineString += " {IMP}";
                        break;
                    case ImmediateAddressing _:
                        {
                            var value = _bus.ReadByte(address);
                            address++;
                            lineString += "#$" + value.ToString("X2") + " {IMM}";
                            break;
                        }

                    case ZeroPageAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + lo.ToString("X2") + " {ZP0}";
                            break;
                        }

                    case ZeroPageXOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + lo.ToString("X2") + ", X {ZPX}";
                            break;
                        }

                    case ZeroPageYOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + lo.ToString("X2") + ", Y {ZPY}";
                            break;
                        }

                    case IndirectXAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "($" + lo.ToString("X2") + ", X) {IZX}";
                            break;
                        }

                    case IndirectYAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "($" + lo.ToString("X2") + "), Y {IZY}";
                            break;
                        }

                    case AbsoluteAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + ((ushort)(hi << 8) | lo).ToString("X4") + " {ABS}";
                            break;
                        }

                    case AbsoluteXOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + ((ushort)((hi << 8) | lo)).ToString("X4") + ", X {ABX}";
                            break;
                        }

                    case AbsoluteYOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + ((ushort)((hi << 8) | lo)).ToString("X4") + ", Y {ABY}";
                            break;
                        }

                    case IndirectAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "($" + ((ushort)(hi << 8) | lo).ToString("X4") + ") {IND}";
                            break;
                        }

                    case RelativeAddressing _:
                        {
                            var value = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + value.ToString("X2") + " [$" + ((ushort)address + value).ToString("X4") + "] {REL}";
                            break;
                        }
                }

                if(!values.ContainsKey(lineAddress))
                    values.Add(lineAddress, lineString);
            }

            return values;
        }

        private void PopulateOpcodeLookup()
        {
            var assembly = typeof(CPU).Assembly;
            var opcodeTypes = assembly.GetTypes().Where(t => t.IsDefined(typeof(OpCodeAttribute), false));

            foreach(var type in opcodeTypes)
            {
                var opcodeAttributes = type.GetCustomAttributes(false).OfType<OpCodeAttribute>();

                foreach(var opcodeAttribute in opcodeAttributes)
                {
                    var addressingStrategy = Activator.CreateInstance(opcodeAttribute.AddressingMode) as IAddressingStrategy;
                    var operatingStrategy = Activator.CreateInstance(type) as IOperationStrategy;
                    var instruction = new Instruction(operatingStrategy.Name, addressingStrategy, operatingStrategy, opcodeAttribute.Cycles);

                    _opcodeLookup.Add(opcodeAttribute.OpCodeAddress, instruction);
                }
            }
        }

        private void PerformInterrupt()
        {
            _bus.Write(Registers.GetStackAddress(), (byte)((Registers.ProgramCounter >> 8) & 0x00FF));
	        Registers.StackPointer--;

	        _bus.Write(Registers.GetStackAddress(), (byte)(Registers.ProgramCounter & 0x00FF));
	        Registers.StackPointer--;

            Registers.StatusRegister.Break = true;
            Registers.StatusRegister.InterruptDisable = true;
	        _bus.Write(Registers.GetStackAddress(), Registers.StatusRegister);
	        Registers.StackPointer--;

	        Registers.ProgramCounter = _bus.ReadWord(0xFFFA);

	        _cycles = 8;
        }
    }
}