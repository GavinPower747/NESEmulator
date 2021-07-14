using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class SetInteruptDisableOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.InterruptDisable = true;

            return 0;
        }
    }
}