using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    ///<summary>
    ///Push the current value of the accumulator onto the stack
    ///</summary>
    public class PushAccumulatorOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            ushort stackAddress = (ushort)(0x0100 + registers.StackPointer);

            bus.Write(stackAddress, registers.Accumulator);

            registers.StackPointer--;

            return 0;
        }
    }
}