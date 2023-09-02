using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x38, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class SetCarryFlagOperation : IOperationStrategy
{
    public string Name => "SEC";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StatusRegister.Carry = true;

        return 0;
    }
}