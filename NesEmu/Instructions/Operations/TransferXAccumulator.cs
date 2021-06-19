using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class TransferXAccumulator : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Accumulator = registers.X;

            return 0;
        }
    }
}