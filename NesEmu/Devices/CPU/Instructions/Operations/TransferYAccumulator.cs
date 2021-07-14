using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class TransferYAccumulator : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Accumulator = registers.Y;

            return 0;
        }
    }
}