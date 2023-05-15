using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x84, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x94, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x8C, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
public class StoreYRegisterOperation : IOperationStrategy
{
    public string Name => "STY";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        bus.Write(address, registers.Y);

        return 0;
    }
}