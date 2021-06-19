using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class TransferAccumulatorX : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.X = registers.Accumulator;

            return 0;
        }
    }
}