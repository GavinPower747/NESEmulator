using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class LogicalOrOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.Read(address);
            registers.Accumulator = (byte)(data | registers.Accumulator);

            registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
            return 1;
        }
    }
}