using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing;

///<summary>
///Instruction requires no additional data.
///</summary>
public class ImpliedAddressing : IAddressingStrategy
{
    public (ushort address, int extraCycles) GetOperationAddress(CpuRegisters registers, IBus bus)
    {
        return (registers.ProgramCounter, 0);
    }
}