using System.Reflection.Emit;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0xBA, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class TransferStackXOperation : IOperationStrategy
{
    public string Name => "TSX";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.X = registers.StackPointer;

        return 0;
    }
}