using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class LogicalShiftRightMemoryOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            byte origVal = bus.Read(address);
            byte shiftedVal = (byte)(origVal / 2);

            registers.StatusRegister.Carry = origVal.GetBitValue(0);
            registers.StatusRegister.SetZeroAndNegative(shiftedVal);

            bus.Write(address, shiftedVal);

            return 0;
        }
    }

    public class LogicalShiftRightAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            byte origVal = registers.Accumulator;
            byte shiftedVal = (byte)(origVal / 2);

            registers.StatusRegister.Carry = origVal.GetBitValue(0);
            registers.StatusRegister.SetZeroAndNegative(shiftedVal);

            bus.Write(address, shiftedVal);

            return 0;
        }
    }
}