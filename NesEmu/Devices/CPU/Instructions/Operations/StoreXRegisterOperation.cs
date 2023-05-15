using System.Reflection.Emit;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x86, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
[OpCode(OpCodeAddress = 0x96, AddressingMode = typeof(ZeroPageYOffsetAddressing), Cycles = 4)]
[OpCode(OpCodeAddress = 0x8E, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
public class StoreXRegisterOperation : IOperationStrategy
{
    public string Name => "STX";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        bus.Write(address, registers.X);

        return 0;
    }
}