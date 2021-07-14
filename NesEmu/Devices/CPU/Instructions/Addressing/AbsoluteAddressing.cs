using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing
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