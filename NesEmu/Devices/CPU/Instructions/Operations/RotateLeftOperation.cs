using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x26, AddressingMode = typeof(ZeroPageAddressing), Cycles = 5)]
[OpCode(OpCodeAddress = 0x36, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x2E, AddressingMode = typeof(AbsoluteAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x3E, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 7)]
public class RotateLeftOperation : IOperationStrategy
{
    public string Name => "ROL";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        var hadCarry = registers.StatusRegister.Carry;
        var value = bus.ReadByte(address);

        registers.StatusRegister.Carry = (value & 0x80) != 0;
        value <<= 1;

        if (hadCarry)
            value |= 1;

        bus.Write(address, value);

        registers.StatusRegister.SetZeroAndNegative(value);
        return 0;
    }
}

//Certain opcodes require this operation to operate on the accumulator
[OpCode(OpCodeAddress = 0x2A, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class RotateLeftAccumulatorOperation : IOperationStrategy
{
    public string Name => "ROL";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        var hadCarry = registers.StatusRegister.Carry;

        registers.StatusRegister.Carry = (registers.Accumulator & 0x80) != 0;
        registers.Accumulator <<= 1;

        if (hadCarry)
            registers.Accumulator |= 1;

        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
        return 0;
    }
}