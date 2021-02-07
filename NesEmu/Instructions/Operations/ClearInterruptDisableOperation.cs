using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearInterruptDisableOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.InterruptDisable, false);

            return 0;
        }
    }
}