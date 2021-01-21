using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    public class IndirectAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var low = bus.Read(registers.ProgramCounter);
            registers.ProgramCounter++;

            var hi = bus.Read(registers.ProgramCounter);
            registers.ProgramCounter++;

            ushort address = (ushort)((hi << 8) | low);

            //Fun Fact: The original hardware actually has a bug in it that developers had to work around
            //since this was a known thing that devs actively worked around we actually have to emulate this
            //bug for an accurate emulation
            //See the NB section of the following link for more details:
            //http://www.obelisk.me.uk/6502/reference.html#JMP    
            if (low == 0x00FF)
	        {
	        	address = (ushort)((bus.Read((ushort)(address & 0xFF00)) << 8) | bus.Read((ushort)(address + 0)));
	        }
	        else
	        {
	        	address = (ushort)((bus.Read((ushort)(address + 1)) << 8) | bus.Read((ushort)(address + 0)));
	        }

            return (address, 0);
        }
    }
}