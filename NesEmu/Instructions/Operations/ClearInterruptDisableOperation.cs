using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class ClearInterruptDisableOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.InterruptDisable = false;

            return 0;
        }
    }
}