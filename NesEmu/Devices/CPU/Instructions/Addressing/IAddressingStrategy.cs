using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    public interface IAddressingStrategy
    {
        (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus);
    }
}