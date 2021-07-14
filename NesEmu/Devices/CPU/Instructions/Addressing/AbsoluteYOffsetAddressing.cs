using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    //adr = arg + Y
    public class AbsoluteYOffsetAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var arg = bus.ReadByte(registers.ProgramCounter);
            registers.ProgramCounter++;

            ushort address = (ushort)(arg + registers.Y);

            if(!address.IsOnSamePageAs(arg))
                return (address, 1);
            else
                return (address, 0);
        }
    }
}