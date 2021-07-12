using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    public class AbsoluteAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var arg = bus.ReadByte(registers.ProgramCounter);

            registers.ProgramCounter++;

            return (arg, 0);
        }
    }
}