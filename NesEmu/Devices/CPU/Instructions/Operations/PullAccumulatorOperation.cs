using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

///<summary>
///Pull an accumulator off the stack
///</summary>
[OpCode(OpCodeAddress = 0x68, AddressingMode = typeof(ImpliedAddressing), Cycles = 4)]
public class PullAccumulatorOperation : IOperationStrategy
{
    public string Name => "PLA";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        registers.StackPointer++;

        registers.Accumulator = bus.ReadByte(registers.GetStackAddress());

        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
        return 0;
    }
}