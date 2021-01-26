using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearDecimalModeOperation : IOperationStrategy
    {
        public int Operate(byte data, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.SetFlag(StatusRegister.Decimal, false);

            return 0;
        }
    }
}