using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    //adr = PEEK(PEEK((arg + Y) % 256) + PEEK((arg + Y + 1) % 256) * 256)
    public class IndirectYAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus) 
        {
            registers.ProgramCounter++;
            var arg = bus.Read(registers.ProgramCounter);

            var lowAddress = (ushort)(arg + registers.Y);
            lowAddress &= 0x00FF;
            var low = bus.Read(lowAddress);

            var hiAddress = (ushort)(arg + registers.Y + 1);
            hiAddress &= 0x00FF;
            var hi = bus.Read(hiAddress);

            var address = (ushort)((hi << 8) | low);
            return (address, 0);
        }
    }
}