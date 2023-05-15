using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x98, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class TransferYAccumulator : IOperationStrategy
{
    public string Name => "TYA";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        registers.Accumulator = registers.Y;

        return 0;
    }
}