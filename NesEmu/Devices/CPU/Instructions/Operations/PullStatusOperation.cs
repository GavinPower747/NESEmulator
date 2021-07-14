using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    ///<summary>
    ///Pulls the status back off of the stack
    ///</summary>
    public class PullStatusOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StackPointer++;

            byte statusRegister = bus.ReadByte(registers.GetStackAddress());

            registers.StatusRegister = new StatusRegister(statusRegister);

            return 0;
        }
    }
}