using System;
using NesEmu.Core;
using NesEmu.Instructions.Addressing;

namespace NesEmu
{
    public class IndirectXAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, CPUBus bus)
        {
            registers.ProgramCounter++;

            ushort lowAddress = Convert.ToUInt16(registers.ProgramCounter + registers.XRegister);
            var low = bus.Read(lowAddress);

            ushort hiAddress = Convert.ToUInt16(registers.ProgramCounter + registers.XRegister + 1);
            var hi = bus.Read(hiAddress);

            ushort address = Convert.ToUInt16((hi << 8) | low);
            return (address, 0);
        }
    }
}