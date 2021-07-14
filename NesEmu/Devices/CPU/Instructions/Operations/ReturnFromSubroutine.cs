using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class ReturnFromSubroutine : IOperationStrategy
    {
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