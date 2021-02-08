using NesEmu.Core;
using NesEmu.Extensions;
using System;

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

            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.Accumulator == 0);
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.Accumulator & 0x80));

            return 0;
        }
    }
}