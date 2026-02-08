using System.Reflection.Emit;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x9A, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
public class TransferXStackOperation : IOperationStrategy
{
    public string Name => "TXS";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StackPointer = registers.X;

        // Note: We purposely don't set any of the status register flags
        // here, as per the 6502 spec. This is to allow manipulating the
        // stack pointer without affecting the flags.

        return 0;
    }
}
