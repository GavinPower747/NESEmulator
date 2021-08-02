using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0x40, AddressingMode = typeof(ImpliedAddressing), Cycles = 6)]
    public class ReturnFromInterrupt : IOperationStrategy
    {
        public string Name => "RTI";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StackPointer++;

            var returnedStatus = bus.ReadByte(registers.GetStackAddress());
            registers.StatusRegister = new StatusRegister(returnedStatus);

            registers.StackPointer++;
            registers.ProgramCounter = bus.ReadWord(registers.GetStackAddress());

            return 0;
        }
    }
}