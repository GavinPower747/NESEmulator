using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    //adr = PEEK(arg) + PEEK((arg + 1) % 256) * 256 + Y	
    public class IndirectYAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus) 
        {
            var arg = bus.ReadByte(registers.ProgramCounter);

            var low = bus.ReadByte(arg);

            var hiAddress = (ushort)(arg + 1);
            hiAddress &= 0x00FF;
            var hi = bus.ReadByte(hiAddress);

            var address = (ushort)(((hi << 8) | low) + registers.Y);
            
            //If the offset results in a change of page this operation
            //will need an extra cycle
            if(!address.IsOnSamePageAs((ushort)(hi << 8)))
                return (address, 1);
            else
                return (address, 0);
        }
    }
}