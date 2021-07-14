using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class ExclusiveOROperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Accumulator ^= bus.ReadByte(address);

            registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);

            return 0;
        }
    }
}