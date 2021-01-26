using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearCarryFlagOperation : IOperationStrategy
    {
        public int Operate(byte data, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.SetFlag(StatusRegister.Carry, false);

            return 0;
        }
    }
}