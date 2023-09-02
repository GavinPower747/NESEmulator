using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x66, AddressingMode = typeof(ZeroPageAddressing), Cycles = 5)]
[OpCode(OpCodeAddress = 0x76, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x6E, AddressingMode = typeof(AbsoluteAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x7E, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 7)]
public class RotateRightOperation : IOperationStrategy
{
    public string Name => "ROR";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        var hadCarry = registers.StatusRegister.Carry;
        var value = bus.ReadByte(address);

        registers.StatusRegister.Carry = (value & 0x80) != 0;
        value >>= 1;

        if (hadCarry)
            value |= 1;

        bus.Write(address, value);

        registers.StatusRegister.SetZeroAndNegative(value);
        return 0;
    }
}

//Certain opcodes require this operation to operate on the accumulator
[OpCode(OpCodeAddress = 0x6A, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class RotateRightAccumulatorOperation : IOperationStrategy
{
    public string Name => "ROR";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        var hadCarry = registers.StatusRegister.Carry;

        registers.StatusRegister.Carry = (registers.Accumulator & 0x80) != 0;
        registers.Accumulator >>= 1;

        if (hadCarry)
            registers.Accumulator |= 1;

        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
        return 0;
    }
}