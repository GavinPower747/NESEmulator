using NesEmu.Core;
using NesEmu.Extensions;
using System;

namespace NesEmu.Instructions.Operations
{
    public class LogicalAndOperation : IOperationStrategy
    {
        public int Operate(byte data, CPURegisters registers, IBus bus)
        {
            registers.Accumulator = (byte)(data & registers.Accumulator);

            registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.Accumulator == 0);
            registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.Accumulator & 0x80));

            return 0;
        }
    }
}