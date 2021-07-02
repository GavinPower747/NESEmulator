using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class ClearDecimalModeOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.Decimal = false;

            return 0;
        }
    }
}