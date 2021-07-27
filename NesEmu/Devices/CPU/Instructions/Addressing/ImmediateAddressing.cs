using System;
using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    ///<summary>
    ///The following memory address will be used as the value for this instruction
    ///</summary>
    public class ImmediateAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            registers.ProgramCounter++;
            return (registers.ProgramCounter, 0);
        }
    }
}