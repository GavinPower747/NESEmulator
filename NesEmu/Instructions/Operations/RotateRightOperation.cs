using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class RotateRightOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var hadCarry = registers.StatusRegister.Carry;
            var value = bus.Read(address);

            registers.StatusRegister.Carry = (value & 0x80) != 0;
            value >>= 1;

            if(hadCarry)
                value |= 1;
            
            bus.Write(address, value);

            registers.StatusRegister.SetZeroAndNegative(value);
            return 0;
        }
    }

    //Certain opcodes require this operation to operate on the accumulator
    public class RotateRightAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var hadCarry = registers.StatusRegister.Carry;

            registers.StatusRegister.Carry = (registers.Accumulator & 0x80) != 0;
            registers.Accumulator >>= 1;

            if(hadCarry)
                registers.Accumulator |= 1;
            
            registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
            return 0;
        }
    }
}