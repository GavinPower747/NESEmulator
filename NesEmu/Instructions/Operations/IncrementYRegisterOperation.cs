using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class IncrementYRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Y += 1;

            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.Y == 0);
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.Y & 0x80));

            return 0;
        }
    }
}