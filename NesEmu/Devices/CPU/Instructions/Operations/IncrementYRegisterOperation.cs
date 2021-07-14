using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class IncrementYRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Y += 1;

            registers.StatusRegister.SetZeroAndNegative(registers.Y);
            return 0;
        }
    }
}