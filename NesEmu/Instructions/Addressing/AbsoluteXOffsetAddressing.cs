using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Addressing
{
    //adr = arg + X
    public class AbsoluteXOffsetAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var arg = bus.ReadByte(registers.ProgramCounter);
            registers.ProgramCounter++;

            ushort address = (ushort)(arg + registers.X);

            if(!address.IsOnSamePageAs(arg))
                return (address, 1);
            else
                return (address, 0);
        }
    }
}