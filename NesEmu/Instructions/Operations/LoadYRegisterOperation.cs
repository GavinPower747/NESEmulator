using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class LoadYRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.ReadByte(address);
            registers.Y = data;

            registers.StatusRegister.SetZeroAndNegative(registers.Y);
            return 0;
        }
    }
}