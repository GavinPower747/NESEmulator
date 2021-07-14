using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class StoreYRegisterOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            bus.Write(address, registers.Y);

            return 0;
        }
    }
}