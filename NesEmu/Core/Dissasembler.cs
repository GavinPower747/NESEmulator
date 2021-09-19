using System.Collections.Generic;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Devices.CPU.Instructions.Operations;

namespace NesEmu.Core
{
    ///<summary>
    ///Expose inner workings of the emulator without actually allowing access to the innards
    ///</summary>
    public class Disassembler
    {
        private IBus _bus;
        private CPU _cpu;

        internal Disassembler(IBus cpuBus, CPU cpu)
        {
            _bus = cpuBus;
            _cpu = cpu;
        }

        public CPURegisters GetCpuRegisterStatus() => _cpu.Registers;

        public Dictionary<ushort, string> GetCPUDisassembly(ushort start, ushort end)
        {
            var values = new Dictionary<ushort, string>();
            var address = start;

            while(address < end)
            {
                Instruction instruction = null;
                var opcode = _bus.ReadByte(address);
                var lineAddress = address;
                var lineString = string.Empty;

                _cpu.OpcodeLookup.TryGetValue(opcode, out instruction);

                if(instruction is null)
                    instruction = new Instruction("NOP", new ImpliedAddressing(), new NoOpOperation(), 2);
                
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

                values.Add(lineAddress, lineString);
            }

            return values;
        }
    }
}