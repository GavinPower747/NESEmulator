using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class ClearCarryFlagOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.SetFlag(StatusRegister.Carry, false);

            return 0;
        }
    }
}