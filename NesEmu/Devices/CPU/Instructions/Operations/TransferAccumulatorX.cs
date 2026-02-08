using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0xAA, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class TransferAccumulatorX : IOperationStrategy
{
    public string Name => "TAX";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.X = registers.Accumulator;
        registers.StatusRegister.SetZeroAndNegative(registers.X);

        return 0;
    }
}
