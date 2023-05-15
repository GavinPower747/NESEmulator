using System.Reflection.Emit;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x49, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
[OpCode(OpCodeAddress = 0x45, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x55, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x4D, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x5D, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x59, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x41, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x51, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
public class ExclusiveOROperation : IOperationStrategy
{
    public string Name => "EOR";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        registers.Accumulator ^= bus.ReadByte(address);

        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);

        return 0;
    }
}