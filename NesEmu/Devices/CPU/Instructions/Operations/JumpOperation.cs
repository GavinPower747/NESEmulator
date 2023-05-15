using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x4C, AddressingMode = typeof(AbsoluteAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x6C, AddressingMode = typeof(IndirectAddressing), Cycles = 5)]
public class JumpOperation : IOperationStrategy
{
    public string Name => "JMP";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        registers.ProgramCounter = address;

        return 0;
    }
}