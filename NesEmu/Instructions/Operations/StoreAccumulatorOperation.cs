using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class StoreAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            bus.Write(address, registers.Accumulator);

            return 0;
        }
    }
}