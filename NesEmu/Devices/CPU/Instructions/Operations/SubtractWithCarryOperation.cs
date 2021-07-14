using System;
using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class SubtractWithCarryOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var fetched = bus.ReadByte(address);

            //Invert the fetched byte and add it
            ushort value = (ushort)(fetched ^ 0x00FF);
            byte carryValue = registers.StatusRegister.Carry ? (byte)1 : (byte)0;
            
            // Notice this is exactly the same as addition from here!
            byte subtracted = (byte)(registers.Accumulator + value + carryValue);
            registers.StatusRegister.Carry = subtracted > 255;
            registers.StatusRegister.SetZeroAndNegative(subtracted);
            registers.StatusRegister.Overflow = Convert.ToBoolean(((~(registers.Accumulator ^ value) & (registers.Accumulator ^ value)) & 0x0080));

            registers.Accumulator = (byte)(subtracted & 0x00FF);
            return 1;
        }
    }
}