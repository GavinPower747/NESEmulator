using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    //adr = PEEK((arg + X) % 256)
    public class ZeroPageXOffsetAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            ushort arg = bus.ReadByte(registers.ProgramCounter);
            ushort offsetArg = (ushort)(arg + registers.X);
            offsetArg &= 0x00FF;

            return (offsetArg, 0);
        }
    }
}