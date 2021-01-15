using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    public interface IAddressingStrategy
    {
        (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, CPUBus bus);
    }
}