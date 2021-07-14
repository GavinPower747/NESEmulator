using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class TransferXStackOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StackPointer = registers.X;

            return 0;
        }
    }
}