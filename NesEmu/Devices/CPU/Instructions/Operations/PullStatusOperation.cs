using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

///<summary>
///Pulls the status back off of the stack
///</summary>
[OpCode(OpCodeAddress = 0x28, AddressingMode = typeof(ImpliedAddressing), Cycles = 4)]
public class PullStatusOperation : IOperationStrategy
{
    public string Name => "PLP";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        registers.StackPointer++;

        byte statusRegister = bus.ReadByte(registers.GetStackAddress());

        registers.StatusRegister = new StatusRegister(statusRegister);

        return 0;
    }
}