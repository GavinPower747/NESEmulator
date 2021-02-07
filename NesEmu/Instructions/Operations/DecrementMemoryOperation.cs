using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class DecrementMemoryOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.Read(address);
            byte newValue = (byte)(data - 1);

            bus.Write(address, newValue);

            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Zero, newValue == 0);
            registers.StatusRegister = registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(newValue & 0x80));

            return 0;
        }
    }
}