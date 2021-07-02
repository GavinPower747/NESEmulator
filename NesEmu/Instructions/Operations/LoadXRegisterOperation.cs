using System;
using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class LoadXRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.Read(address);
            registers.X = data;

            registers.StatusRegister.SetZeroAndNegative(registers.X);
            return 0;
        }
    }
}