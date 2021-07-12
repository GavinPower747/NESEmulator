using System;
using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class DecrementMemoryOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.ReadByte(address);
            byte newValue = (byte)(data - 1);

            bus.Write(address, newValue);

            registers.StatusRegister.SetZeroAndNegative(newValue);

            return 0;
        }
    }
}