using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

///<summary>
///Push the current value of the accumulator onto the stack
///</summary>
[OpCode(OpCodeAddress = 0x48, AddressingMode = typeof(ImpliedAddressing), Cycles = 3)]
public class PushAccumulatorOperation : IOperationStrategy
{
    public string Name => "PHA";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        ushort stackAddress = (ushort)(0x0100 + registers.StackPointer);

        bus.Write(stackAddress, registers.Accumulator);

        registers.StackPointer--;

        return 0;
    }
}