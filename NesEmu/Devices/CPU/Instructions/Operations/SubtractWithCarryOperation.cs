using System;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0xE9, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
[OpCode(OpCodeAddress = 0xE5, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0xF5, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0xED, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0xFD, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0xF9, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0xE1, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0xF1, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
public class SubtractWithCarryOperation : IOperationStrategy
{
    public string Name => "SBC";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        var fetched = bus.ReadByte(address);

        // SBC is implemented as A + ~M + C (one's complement addition)
        int invertedValue = fetched ^ 0xFF;
        int carryValue = registers.StatusRegister.Carry ? 1 : 0;

        int sum = registers.Accumulator + invertedValue + carryValue;
        byte result = (byte)(sum & 0xFF);

        // Carry is set if no borrow was needed (sum >= 256 in the addition view)
        registers.StatusRegister.Carry = sum > 255;
        registers.StatusRegister.SetZeroAndNegative(result);

        // Overflow: same formula as ADC but with inverted operand
        registers.StatusRegister.Overflow =
            ((registers.Accumulator ^ result) & (invertedValue ^ result) & 0x80) != 0;

        registers.Accumulator = result;
        return 1;
    }
}
