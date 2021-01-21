using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    //adr = arg + X
    public class AbsoluteXOffsetAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var arg = bus.Read(registers.ProgramCounter);
            registers.ProgramCounter++;

            ushort address = (ushort)(arg + registers.X);

            if((address & 0xFF00) != (arg & 0xFF00))
                return (address, 1);
            else
                return (address, 0);
        }
    }
}