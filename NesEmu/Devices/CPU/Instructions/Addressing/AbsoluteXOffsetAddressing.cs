using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU.Instructions.Addressing;

///<summary>
///Read the full 16 bit address and offset the read value
///by the value in the X register
///</summary>
public class AbsoluteXOffsetAddressing : IAddressingStrategy
{
    public (ushort address, int extraCycles) GetOperationAddress(CpuRegisters registers, IBus bus)
    {
        var arg = bus.ReadWord(registers.ProgramCounter);
        registers.ProgramCounter += 2;

        ushort address = (ushort)(arg + registers.X);

        if (!address.IsOnSamePageAs(arg))
            return (address, 1);
        else
            return (address, 0);
    }
}