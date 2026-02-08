using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations;

[OpCode(OpCodeAddress = 0x40, AddressingMode = typeof(ImpliedAddressing), Cycles = 6)]
public class ReturnFromInterrupt : IOperationStrategy
{
    public string Name => "RTI";

    public int Operate(ushort address, CpuRegisters registers, IBus bus)
    {
        // Pull status from stack
        registers.StackPointer++;
        var returnedStatus = bus.ReadByte(registers.GetStackAddress());

        // Bit 5 always set, bit 4 (Break) ignored on pull
        returnedStatus = (byte)((returnedStatus | 0x20) & ~0x10);
        registers.StatusRegister = new StatusRegister(returnedStatus);

        // Pull PC low byte
        registers.StackPointer++;
        byte pcLo = bus.ReadByte(registers.GetStackAddress());

        // Pull PC high byte
        registers.StackPointer++;
        byte pcHi = bus.ReadByte(registers.GetStackAddress());

        registers.ProgramCounter = (ushort)((pcHi << 8) | pcLo);

        return 0;
    }
}
