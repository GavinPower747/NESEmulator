using System;
using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class DecrementYRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Y -= 1;

            registers.StatusRegister.SetZeroAndNegative(registers.Y);

            return 0;
        }
    }
}