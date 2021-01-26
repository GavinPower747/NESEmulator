using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearInterruptDisableOperation : IOperationStrategy
    {
        public int Operate(byte data, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.SetFlag(StatusRegister.InterruptDisable, false);

            return 0;
        }
    }
}