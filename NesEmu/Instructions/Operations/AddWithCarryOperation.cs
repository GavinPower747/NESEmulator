using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class AddWithCarryOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var value = bus.Read(address);
            ushort carryValue = registers.StatusRegister.HasFlag(StatusRegister.Carry) ? (ushort)0x01 : (ushort)0x00;

            ushort added = Convert.ToUInt16((ushort)registers.Accumulator + (ushort)value + carryValue);

            registers.StatusRegister.SetFlag(StatusRegister.Carry, added > 255);
            registers.StatusRegister.SetFlag(StatusRegister.Zero, Convert.ToBoolean((added & 0x00FF) == 0));
            registers.StatusRegister.SetFlag(StatusRegister.Negative, Convert.ToBoolean(added & 0x80));
            registers.StatusRegister.SetFlag(StatusRegister.Overflow, Convert.ToBoolean(((~((ushort)registers.Accumulator ^ (ushort)value) & ((ushort)registers.Accumulator ^ (ushort)value)) & 0x0080)));

            registers.Accumulator = Convert.ToByte(added & 0x00FF);

            return 1;
        }
    }
}