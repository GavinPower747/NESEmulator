using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class ClearCarryFlagOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.Carry = false;

            return 0;
        }
    }
}