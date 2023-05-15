using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;
using System;

namespace NesEmu.Devices.CPU.Instructions.Operations;

///<summary>
///Push the value of the status register onto the stack
///</summary>
[OpCode(OpCodeAddress = 0x08, AddressingMode = typeof(ImpliedAddressing), Cycles = 3)]
public class PushProcessorStatusOperation : IOperationStrategy
{
    public string Name => "PHP";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        ushort stackAddress = (ushort)(0x0100 + registers.StackPointer);

        bus.Write(stackAddress, Convert.ToByte(registers.StatusRegister));

        registers.StatusRegister.Break = false;
        registers.StatusRegister.B = false;

        registers.StackPointer--;

        return 0;
    }
}