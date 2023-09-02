using System.Collections.Generic;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Devices.CPU.Instructions.Operations;

namespace NesEmu.Core;

///<summary>
///Expose inner workings of the emulator without actually allowing access to the innards
///</summary>
public class Disassembler
{
    private readonly IBus _cpuBus;
    private readonly IBus _ppuBus;
    private readonly Cpu _cpu;

    internal Disassembler(IBus cpuBus, IBus ppuBus, Cpu cpu)
    {
        _cpuBus = cpuBus;
        _ppuBus = ppuBus;
        _cpu = cpu;
    }

    public CpuRegisters GetCpuRegisterStatus() => _cpu.Registers;

    public Dictionary<ushort, string> GetCPUDisassembly(ushort start, ushort end)
    {
        var values = new Dictionary<ushort, string>();
        var address = start;

        while (address < end)
        {
            var opcode = _cpuBus.ReadByte(address);
            var lineAddress = address;
            _cpu.OpcodeLookup.TryGetValue(opcode, out Instruction instruction);

            instruction ??= new Instruction("NOP", new ImpliedAddressing(), new NoOpOperation(), 2);

            address++;

            string lineString = instruction.Name + "  ";
            switch (instruction.AddressingStrategy)
            {
                case ImpliedAddressing _:
                    lineString += " {IMP}";
                    break;
                case ImmediateAddressing _:
                    {
                        var value = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "#$" + value.ToString("X2") + " {IMM}";
                        break;
                    }

                case ZeroPageAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "$" + lo.ToString("X2") + " {ZP0}";
                        break;
                    }

                case ZeroPageXOffsetAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "$" + lo.ToString("X2") + ", X {ZPX}";
                        break;
                    }

                case ZeroPageYOffsetAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "$" + lo.ToString("X2") + ", Y {ZPY}";
                        break;
                    }

                case IndirectXAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "($" + lo.ToString("X2") + ", X) {IZX}";
                        break;
                    }

                case IndirectYAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "($" + lo.ToString("X2") + "), Y {IZY}";
                        break;
                    }

                case AbsoluteAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        var hi = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "$" + ((ushort)(hi << 8) | lo).ToString("X4") + " {ABS}";
                        break;
                    }

                case AbsoluteXOffsetAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        var hi = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "$" + ((ushort)((hi << 8) | lo)).ToString("X4") + ", X {ABX}";
                        break;
                    }

                case AbsoluteYOffsetAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        var hi = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "$" + ((ushort)((hi << 8) | lo)).ToString("X4") + ", Y {ABY}";
                        break;
                    }

                case IndirectAddressing _:
                    {
                        var lo = _cpuBus.ReadByte(address);
                        address++;
                        var hi = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "($" + ((ushort)(hi << 8) | lo).ToString("X4") + ") {IND}";
                        break;
                    }

                case RelativeAddressing _:
                    {
                        var value = _cpuBus.ReadByte(address);
                        address++;
                        lineString += "$" + value.ToString("X2") + " [$" + (address + value).ToString("X4") + "] {REL}";
                        break;
                    }
            }

            values.Add(lineAddress, lineString);
        }

        return values;
    }

    public void GetPPUPatternTable(int index)
    {
        for (int y = 0; y < 16; y++)
            for (int x = 0; x < 16; x++)
            {
                ushort byteOffset = (ushort)(y * 256 + x * 16);
                for (ushort row = 0; row < 8; row++)
                {
                    byte tileLo = _ppuBus.ReadByte((ushort)(index * 0x1000 + byteOffset + row));
                    byte tileHi = _ppuBus.ReadByte((ushort)(index * 0x1000 + byteOffset + row + 8));

                    for (int col = 0; col < 8; col++)
                    {
                        byte pixel = (byte)((tileLo & 0x01) + (tileHi & 0x01));
                        tileLo >>= 1;
                        tileHi >>= 1;
                    }
                }
            }
    }
}