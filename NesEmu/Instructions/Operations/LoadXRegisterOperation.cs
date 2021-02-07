using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class LoadXRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.Read(address);
            registers.X = data;

            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Zero, registers.X == 0);
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(registers.X & 0x80));

            return 0;
        }
    }
}