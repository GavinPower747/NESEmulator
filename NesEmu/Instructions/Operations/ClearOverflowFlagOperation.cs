using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class ClearOverflowFlagOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.Overflow = false;

            return 0;
        }
    }
}