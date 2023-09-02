using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0xF8, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class SetDecimalFlagOperation : IOperationStrategy
{
    public string Name => "SED";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StatusRegister.Decimal = true;

        return 0;
    }
}