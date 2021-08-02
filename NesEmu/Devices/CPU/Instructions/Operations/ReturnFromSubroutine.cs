using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0x60, AddressingMode = typeof(ImpliedAddressing), Cycles = 6)]
    public class ReturnFromSubroutine : IOperationStrategy
    {
        public string Name => "RTS";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StackPointer++;

            bus.ReadWord(registers.GetStackAddress());

            registers.StackPointer++;
            registers.ProgramCounter++;

            return 0;
        }
    }
}