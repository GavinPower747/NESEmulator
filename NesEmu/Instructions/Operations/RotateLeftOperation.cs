using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class RotateLeftOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            return 0;
        }
    }
}