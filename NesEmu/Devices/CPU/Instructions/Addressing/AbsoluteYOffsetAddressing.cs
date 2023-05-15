using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU.Instructions.Addressing;

///<summary>
///Read the full 16 bit address and offset the read value
///by the value in the Y register
///</summary>
public class AbsoluteYOffsetAddressing : IAddressingStrategy
{
    public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
    {
        var arg = bus.ReadWord(registers.ProgramCounter);
        registers.ProgramCounter += 2;

        ushort address = (ushort)(arg + registers.Y);

        if (!address.IsOnSamePageAs(arg))
            return (address, 1);
        else
            return (address, 0);
    }
}