using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class SetCarryFlagOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.Carry = true;

            return 0;
        }
    }
}