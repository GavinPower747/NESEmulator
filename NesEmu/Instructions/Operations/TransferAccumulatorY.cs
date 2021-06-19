using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class TransferAccumulatorY : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Y = registers.Accumulator;

            return 0;
        }
    }
}