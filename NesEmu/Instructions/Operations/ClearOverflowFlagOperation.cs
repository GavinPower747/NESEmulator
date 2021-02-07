using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearOverflowFlagOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Overflow, false);

            return 0;
        }
    }
}