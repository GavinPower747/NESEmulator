using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    ///<summary>
    ///Pull an accumulator off the stack
    ///</summary>
    public class PullAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StackPointer++;

            registers.Accumulator = bus.Read((byte)(0x0100 + registers.StackPointer));

            registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
            return 0;
        }
    }
}