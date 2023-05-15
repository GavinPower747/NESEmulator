using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x85, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x95, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x8D, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x9D, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 5)]
[OpCode(OpCodeAddress = 0x99, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 5)]
[OpCode(OpCodeAddress = 0x81, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x91, AddressingMode = typeof(IndirectYAddressing), Cycles = 6)]
public class StoreAccumulatorOperation : IOperationStrategy
{
    public string Name => "STA";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        bus.Write(address, registers.Accumulator);

        return 0;
    }
}