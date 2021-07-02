using NesEmu.Core;
using System;

namespace NesEmu.Instructions.Operations
{
    ///<summary>
    ///Push the value of the status register onto the stack
    ///</summary>
    public class PushProcessorStatusOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            ushort stackAddress = (ushort)(0x0100 + registers.StackPointer);

            bus.Write(stackAddress, Convert.ToByte(registers.StatusRegister));

            registers.StatusRegister.Break = false;
            registers.StatusRegister.B = false;

            registers.StackPointer--;

            return 0;
        }
    }
}