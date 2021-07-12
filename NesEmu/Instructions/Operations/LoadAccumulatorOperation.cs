using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class LoadAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.ReadByte(address);
            registers.Accumulator = data;

            registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
            return 0;
        }
    }
}