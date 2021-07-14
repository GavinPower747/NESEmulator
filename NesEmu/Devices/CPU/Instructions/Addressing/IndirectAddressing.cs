using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    public class IndirectAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var address = bus.ReadWord(registers.ProgramCounter);
            registers.ProgramCounter += 2;

            //Fun Fact: The original hardware actually has a bug in it that developers had to work around
            //since this was a known thing that devs actively worked around we actually have to emulate this
            //bug for an accurate emulation
            //See the NB section of the following link for more details:
            //http://www.obelisk.me.uk/6502/reference.html#JMP    
            if ((address & 0x00FF) == 0x00FF)
	        {
	        	address = (ushort)((bus.ReadByte((ushort)(address & 0xFF00)) << 8) | bus.ReadByte((ushort)(address + 0)));
	        }
	        else
	        {
	        	address = (ushort)((bus.ReadByte((ushort)(address + 1)) << 8) | bus.ReadByte((ushort)(address + 0)));
	        }

            return (address, 0);
        }
    }
}