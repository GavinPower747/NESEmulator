using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x24, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x2C, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
public class BitTestOperation : IOperationStrategy
{
    public string Name => "BIT";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        var memoryVal = bus.ReadByte(address);

        registers.StatusRegister.Zero = (memoryVal & registers.Accumulator) == 0;

        registers.StatusRegister.Overflow = memoryVal.GetBitValue(6);
        registers.StatusRegister.Negative = memoryVal.GetBitValue(7);

        return 0;
    }
}