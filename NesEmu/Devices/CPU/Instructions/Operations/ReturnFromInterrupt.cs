using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class ReturnFromInterrupt : IOperationStrategy
    {
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