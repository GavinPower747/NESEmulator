using System;
using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    public class IndirectXAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            registers.ProgramCounter++;

            ushort lowAddress = Convert.ToUInt16(registers.ProgramCounter + registers.X);
            var low = bus.Read(lowAddress);

            ushort hiAddress = Convert.ToUInt16(registers.ProgramCounter + registers.X + 1);
            var hi = bus.Read(hiAddress);

            ushort address = Convert.ToUInt16((hi << 8) | low);
            return (address, 0);
        }
    }
}