using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x29, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
[OpCode(OpCodeAddress = 0x25, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x35, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x2D, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x3D, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x39, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x21, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x31, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
public class LogicalAndOperation : IOperationStrategy
{
    public string Name => "AND";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        var data = bus.ReadByte(address);
        registers.Accumulator = (byte)(data & registers.Accumulator);

        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
        return 0;
    }
}