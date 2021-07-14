using System;
using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class LoadXRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.ReadByte(address);
            registers.X = data;

            registers.StatusRegister.SetZeroAndNegative(registers.X);
            return 0;
        }
    }
}