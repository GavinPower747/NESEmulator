using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    //adr = (arg + Y) % 256)
    public class ZeroPageYOffsetAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            ushort arg = bus.ReadByte(registers.ProgramCounter);
            registers.ProgramCounter++;
            ushort offsetArg = (ushort)(arg + registers.Y);
            offsetArg &= 0x00FF;

            return (offsetArg, 0);
        }
    }
}