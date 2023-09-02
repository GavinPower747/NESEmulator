using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;


[OpCode(OpCodeAddress = 0x18, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class ClearCarryFlagOperation : IOperationStrategy
{
    public string Name => "CLC";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StatusRegister.Carry = false;

        return 0;
    }
}

[OpCode(OpCodeAddress = 0xD8, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class ClearDecimalModeOperation : IOperationStrategy
{
    public string Name => "CLD";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StatusRegister.Decimal = false;

        return 0;
    }
}

[OpCode(OpCodeAddress = 0x58, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class ClearInterruptDisableOperation : IOperationStrategy
{
    public string Name => "CLI";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StatusRegister.InterruptDisable = false;

        return 0;
    }
}

[OpCode(OpCodeAddress = 0xB8, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class ClearOverflowFlagOperation : IOperationStrategy
{
    public string Name => "CLV";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StatusRegister.Overflow = false;

        return 0;
    }
}