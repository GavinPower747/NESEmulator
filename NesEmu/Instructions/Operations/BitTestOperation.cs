using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class BitTestOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var memoryVal = bus.ReadByte(address);

            registers.StatusRegister.Zero = (memoryVal & registers.Accumulator) == 0;

            registers.StatusRegister.Overflow = memoryVal.GetBitValue(6);
            registers.StatusRegister.Negative = memoryVal.GetBitValue(7);

            return 0;
        }
    }
}