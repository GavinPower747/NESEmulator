using System;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

///<summary>
///Push the value of the status register onto the stack
///</summary>
[OpCode(OpCodeAddress = 0x08, AddressingMode = typeof(ImpliedAddressing), Cycles = 3)]
public class PushProcessorStatusOperation : IOperationStrategy
{
    public string Name => "PHP";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        ushort stackAddress = (ushort)(0x0100 + registers.StackPointer);

        // We need to push the status with the break and b flag set to 1, as per the 6502 behavior
        byte modifiedStatus = (byte)(registers.StatusRegister | 0x30); // Set the break flag and B flag to 1
        bus.Write(stackAddress, modifiedStatus);

        registers.StackPointer--;

        return 0;
    }
}
