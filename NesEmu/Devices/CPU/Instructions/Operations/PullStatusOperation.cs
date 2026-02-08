using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

///<summary>
///Pulls the status back off of the stack
///</summary>
///<remarks>
/// We should always clear the break flag when we're pulling
/// back from the stack.
/// </remarks>
[OpCode(OpCodeAddress = 0x28, AddressingMode = typeof(ImpliedAddressing), Cycles = 4)]
public class PullStatusOperation : IOperationStrategy
{
    public string Name => "PLP";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        registers.StackPointer++;

        byte statusRegister = bus.ReadByte(registers.GetStackAddress());

        // Bit 5 should always be set, and bit 4 is ignored on pull
        statusRegister = (byte)((statusRegister | 0x20) & ~0x10);
        registers.StatusRegister = new StatusRegister(statusRegister);

        return 0;
    }
}
