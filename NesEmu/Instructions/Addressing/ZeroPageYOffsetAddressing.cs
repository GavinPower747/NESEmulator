using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    //adr = PEEK((arg + Y) % 256)
    public class ZeroPageYOffsetAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            ushort arg = bus.Read(registers.ProgramCounter);
            ushort offsetArg = (ushort)(arg + registers.Y);
            offsetArg &= 0x00FF;

            return (offsetArg, 0);
        }
    }
}