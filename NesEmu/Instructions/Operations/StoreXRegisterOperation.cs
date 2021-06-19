using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class StoreXRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            bus.Write(address, registers.X);

            return 0;
        }
    }
}