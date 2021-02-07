using NesEmu.Core;
using NesEmu.Extensions;
using System;

namespace NesEmu.Instructions.Operations
{
    public class DecrementXRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.X -= 1;

            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.X == 0);
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.X & 0x80));

            return 0;
        }
    }
}