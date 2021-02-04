using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class LogicalOrOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.Read(address);
            registers.Accumulator = (byte)(data | registers.Accumulator);

            registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.Accumulator == 0x00);
            registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.Accumulator & 0x80));

            return 1;
        }
    }
}