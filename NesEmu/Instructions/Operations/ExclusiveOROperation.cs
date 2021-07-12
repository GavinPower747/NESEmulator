using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class ExclusiveOROperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Accumulator ^= bus.Read(address);

            registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);

            return 0;
        }
    }
}