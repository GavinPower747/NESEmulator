using System;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x69, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
[OpCode(OpCodeAddress = 0x65, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x75, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x6D, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x7D, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x79, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x61, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x71, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
public class AddWithCarryOperation : IOperationStrategy
{
    public string Name => "ADC";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        var value = bus.ReadByte(address);
        var carryValue = registers.StatusRegister.Carry ? (byte)1 : (byte)0;
        var added = (byte)(registers.Accumulator + value + carryValue);

        registers.StatusRegister.Carry = added > 255;
        registers.StatusRegister.SetZeroAndNegative(added);
        registers.StatusRegister.Overflow = Convert.ToBoolean(((~(registers.Accumulator ^ value) & (registers.Accumulator ^ value)) & 0x0080));

        registers.Accumulator = Convert.ToByte(added & 0x00FF);

        return 1;
    }
}