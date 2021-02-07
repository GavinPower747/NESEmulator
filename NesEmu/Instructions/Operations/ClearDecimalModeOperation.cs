using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearDecimalModeOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Decimal, false);

            return 0;
        }
    }
}