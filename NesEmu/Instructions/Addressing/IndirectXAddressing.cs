using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    //adr = PEEK(arg + X) % 256) + PEEK((arg + X + 1) % 256) * 256
    public class IndirectXAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var arg = bus.Read(registers.ProgramCounter);

            ushort lowAddress = (ushort)(arg + registers.X);
            lowAddress &= 0x00FF;
            var low = bus.Read(lowAddress);

            ushort hiAddress = (ushort)(arg + registers.X + 1);
            hiAddress &= 0x00FF;
            var hi = bus.Read(hiAddress);

            ushort address = (ushort)((hi << 8) | low);
            return (address, 0);
        }
    }
}