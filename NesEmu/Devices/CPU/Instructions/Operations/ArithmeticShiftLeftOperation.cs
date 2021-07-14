using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    //A,Z,C,N = M*2 or M,Z,C,N = M*2
    public class ArithmeticShiftLeftOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            byte memValue = bus.ReadByte(address);
            memValue <<= 1;
            registers.StatusRegister.Carry = (memValue & 0xFF00) > 0;
	        registers.StatusRegister.SetZeroAndNegative(memValue);
            
            bus.Write(address, memValue);
            return 0;
        }
    }

    //When accessed using a specific value this operates on the accumulator
    public class ArithmeticShiftLeftAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Accumulator <<= 1;
            registers.StatusRegister.Carry = (registers.Accumulator & 0xFF00) > 0;
	        registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
            
            return 0;
        }
    }
}