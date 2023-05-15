using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

//A,Z,C,N = M*2 or M,Z,C,N = M*2
[OpCode(OpCodeAddress = 0x06, AddressingMode = typeof(ZeroPageAddressing), Cycles = 5)]
[OpCode(OpCodeAddress = 0x16, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x0E, AddressingMode = typeof(AbsoluteAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0x1E, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 7)]
public class ArithmeticShiftLeftOperation : IOperationStrategy
{
    public string Name => "ASL";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        byte memValue = bus.ReadByte(address);
        memValue <<= 1;
        registers.StatusRegister.Carry = (memValue & 0xFF00) > 0;
        registers.StatusRegister.SetZeroAndNegative(memValue);

        bus.Write(address, memValue);
        return 0;
    }
}

//When accessed using a specific value this operates on the accumulator
[OpCode(OpCodeAddress = 0x0A, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class ArithmeticShiftLeftAccumulatorOperation : IOperationStrategy
{
    public string Name => "ASL";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        registers.Accumulator <<= 1;
        registers.StatusRegister.Carry = (registers.Accumulator & 0xFF00) > 0;
        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);

        return 0;
    }
}