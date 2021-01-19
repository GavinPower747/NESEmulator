using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    public class IndirectYAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus) 
        {
            registers.ProgramCounter++;

            var lowAddress = (ushort)(registers.ProgramCounter + registers.Y);
            var low = bus.Read(lowAddress);

            var hiAddress = (ushort)(registers.ProgramCounter + (registers.Y + 1));
            var hi = bus.Read(hiAddress);

            var address = (ushort)((hi << 8) | low);
            return (address, 0);
        }
    }
}