using System;
using NesEmu.Core;
using NesEmu.Extensions;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

public abstract class BranchOperation : IOperationStrategy
{
    public abstract string Name { get; }

    public abstract int Operate(ushort address, CpuRegisters registers, IBus bus);

    protected int PerformOperation(Func<CpuRegisters, bool> predicate, ushort address, CpuRegisters registers, IBus bus)
    {
        if (!predicate(registers))
            return 0;

        var initialProgramCounter = registers.ProgramCounter;
        registers.ProgramCounter = address;

        if (initialProgramCounter.IsOnSamePageAs(registers.ProgramCounter))
            return 1;
        else
            return 2;
    }
}

[OpCode(OpCodeAddress = 0x90, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchCarryClear : BranchOperation
{
    public override string Name => "BCC";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => !reg.StatusRegister.Carry, address, registers, bus);
}

[OpCode(OpCodeAddress = 0xB0, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchCarrySet : BranchOperation
{
    public override string Name => "BCS";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => !reg.StatusRegister.Carry, address, registers, bus);
}

[OpCode(OpCodeAddress = 0xF0, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchIfEqual : BranchOperation
{
    public override string Name => "BEQ";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => reg.StatusRegister.Zero, address, registers, bus);
}

[OpCode(OpCodeAddress = 0x30, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchIfMinus : BranchOperation
{
    public override string Name => "BMI";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => reg.StatusRegister.Negative, address, registers, bus);
}

[OpCode(OpCodeAddress = 0xD0, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchIfNotEqual : BranchOperation
{
    public override string Name => "BNE";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => !reg.StatusRegister.Zero, address, registers, bus);
}

[OpCode(OpCodeAddress = 0x10, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchIfPositive : BranchOperation
{
    public override string Name => "BPL";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => !reg.StatusRegister.Negative, address, registers, bus);
}

[OpCode(OpCodeAddress = 0x50, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchIfOverflowClear : BranchOperation
{
    public override string Name => "BVC";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => !reg.StatusRegister.Overflow, address, registers, bus);
}

[OpCode(OpCodeAddress = 0x70, AddressingMode = typeof(RelativeAddressing), Cycles = 2)]
public class BranchIfOverflowSet : BranchOperation
{
    public override string Name => "BVS";

    public override int Operate(ushort address, CpuRegisters registers, IBus bus)
        => PerformOperation(reg => reg.StatusRegister.Overflow, address, registers, bus);
}