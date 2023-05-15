using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing;

///<summary>
///Read the full 16 bit address and increment the Program Counter for every byte read
///</summary>
public class AbsoluteAddressing : IAddressingStrategy
{
    public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
    {
        var arg = bus.ReadWord(registers.ProgramCounter);

        registers.ProgramCounter += 2;

        return (arg, 0);
    }
}