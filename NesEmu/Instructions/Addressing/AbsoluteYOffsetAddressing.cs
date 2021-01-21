using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    //adr = arg + Y
    public class AbsoluteYOffsetAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var arg = bus.Read(registers.ProgramCounter);
            registers.ProgramCounter++;

            ushort address = (ushort)(arg + registers.Y);

            if((address & 0xFF00) != (arg & 0xFF00))
                return (address, 1);
            else
                return (address, 0);
        }
    }
}