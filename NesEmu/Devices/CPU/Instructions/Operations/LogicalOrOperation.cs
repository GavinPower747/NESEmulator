using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x09, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
[OpCode(OpCodeAddress = 0x05, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x15, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x0D, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x1D, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x19, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x01, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x11, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
public class LogicalOrOperation : IOperationStrategy
{
    public string Name => "ORA";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        var data = bus.ReadByte(address);
        registers.Accumulator = (byte)(data | registers.Accumulator);

        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
        return 1;
    }
}