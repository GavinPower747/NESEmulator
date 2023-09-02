using System;
using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing;

///<summary>
///The following memory address will be used as the value for this instruction
///</summary>
public class ImmediateAddressing : IAddressingStrategy
{
    public (ushort address, int extraCycles) GetOperationAddress(CpuRegisters registers, IBus bus)
    {
        var address = registers.ProgramCounter;
        registers.ProgramCounter++;

        return (address, 0);
    }
}