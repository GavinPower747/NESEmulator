using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class TransferStackXOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.X = registers.StackPointer;

            return 0;    
        }
    }
}