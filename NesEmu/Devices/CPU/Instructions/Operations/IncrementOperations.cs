using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0xE6, AddressingMode = typeof(ZeroPageAddressing), Cycles = 5)]
[OpCode(OpCodeAddress = 0xF6, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0xEE, AddressingMode = typeof(AbsoluteAddressing), Cycles = 6)]
[OpCode(OpCodeAddress = 0xFE, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 7)]
public class IncrementMemoryOperation : IOperationStrategy
{
    public string Name => "INC";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        var data = bus.ReadByte(address);
        byte newValue = (byte)(data + 1);

        bus.Write(address, newValue);

        registers.StatusRegister.SetZeroAndNegative(newValue);
        return 0;
    }
}

[OpCode(OpCodeAddress = 0xE8, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class IncrementXRegisterOperation : IOperationStrategy
{
    public string Name => "INX";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.X += 1;

        registers.StatusRegister.SetZeroAndNegative(registers.X);

        return 0;
    }
}

[OpCode(OpCodeAddress = 0xC8, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class IncrementYRegisterOperation : IOperationStrategy
{
    public string Name => "INY";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.Y += 1;

        registers.StatusRegister.SetZeroAndNegative(registers.Y);
        return 0;
    }
}