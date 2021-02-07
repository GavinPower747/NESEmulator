using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class LoadAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.Read(address);
            registers.Accumulator = data;

            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.Accumulator == 0);
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.Accumulator & 0x80));

            return 0;
        }
    }
}