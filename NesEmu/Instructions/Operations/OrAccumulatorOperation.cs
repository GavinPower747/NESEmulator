using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class OrAccumulatorOperation : IOperationStrategy
    {
        public int Operate(byte data, CPURegisters registers, CPUBus bus)
        {
            registers.Accumulator = Convert.ToByte(data | registers.Accumulator);

            registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.Accumulator == 0x00);
            registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.Accumulator & 0x80));

            return 1;
        }
    }
}