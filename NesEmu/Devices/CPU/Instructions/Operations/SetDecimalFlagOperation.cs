using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class SetDecimalFlagOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.Decimal = true;

            return 0;
        }
    }
}