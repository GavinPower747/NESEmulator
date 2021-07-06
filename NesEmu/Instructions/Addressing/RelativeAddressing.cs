using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    public class RelativeAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var offset = bus.Read(registers.ProgramCounter);
	        
            ushort valueAddress = (ushort)(registers.ProgramCounter + 1 + offset);
            
            if (offset >= 0x80)
                valueAddress -= 0x100;
            
            return (valueAddress, 0);
        }
    }
}