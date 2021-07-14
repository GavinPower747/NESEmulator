using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class DecrementXRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.X -= 1;

            registers.StatusRegister.SetZeroAndNegative(registers.X);
            return 0;
        }
    }
}