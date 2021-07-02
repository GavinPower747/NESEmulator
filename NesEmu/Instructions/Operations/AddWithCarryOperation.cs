using System;
using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class AddWithCarryOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var value = bus.Read(address);
            var carryValue = registers.StatusRegister.Carry ? (byte)1 : (byte)0;
            var added = (byte)(registers.Accumulator + value + carryValue);

            registers.StatusRegister.Carry = added > 255;
            registers.StatusRegister.SetZeroAndNegative(added);
            registers.StatusRegister.Overflow = Convert.ToBoolean(((~(registers.Accumulator ^ value) & (registers.Accumulator ^ value)) & 0x0080));

            registers.Accumulator = Convert.ToByte(added & 0x00FF);

            return 1;
        }
    }
}