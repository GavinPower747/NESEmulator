using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0xA2, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
[OpCode(OpCodeAddress = 0xA6, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0xB6, AddressingMode = typeof(ZeroPageYOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0xAE, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0xBE, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
public class LoadXRegisterOperation : IOperationStrategy
{
    public string Name => "LDX";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        var data = bus.ReadByte(address);
        registers.X = data;

        registers.StatusRegister.SetZeroAndNegative(registers.X);
        return 0;
    }
}