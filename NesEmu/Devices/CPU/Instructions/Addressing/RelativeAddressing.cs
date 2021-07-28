using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    public class RelativeAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var offset = bus.ReadByte(registers.ProgramCounter);

            registers.ProgramCounter++;

            ushort valueAddress = (ushort)(registers.ProgramCounter + offset);

            if (offset >= 0x80)
                valueAddress -= 0x100;
            
            return (valueAddress, 0);
        }
    }
}