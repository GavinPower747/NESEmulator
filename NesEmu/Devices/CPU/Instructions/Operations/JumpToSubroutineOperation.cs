using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x20, AddressingMode = typeof(AbsoluteAddressing), Cycles = 6)]
public class JumpToSubroutineOperation : IOperationStrategy
{
    public string Name => "JSR";

    public int Operate(ushort address, CPURegisters registers, IBus bus)
    {
        registers.ProgramCounter--;

        bus.Write((ushort)(0x0100 + registers.StackPointer), (byte)((registers.ProgramCounter >> 8) & 0x00FF));
        registers.StackPointer--;
        bus.Write((ushort)(0x0100 + registers.StackPointer), (byte)(registers.ProgramCounter & 0x00FF));
        registers.StackPointer--;

        registers.ProgramCounter = address;
        return 0;
    }
}