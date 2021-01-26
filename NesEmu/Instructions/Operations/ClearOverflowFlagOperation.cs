using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearOverflowFlagOperation : IOperationStrategy
    {
        public int Operate(byte data, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.SetFlag(StatusRegister.Overflow, false);

            return 0;
        }
    }
}