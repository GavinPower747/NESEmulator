using NesEmu.Core;
using System;

namespace NesEmu.Instructions.Operations
{
    ///<summary>
    ///Pulls the status back off of the stack
    ///</summary>
    public class PullStatusOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StackPointer++;

            byte statusRegister = bus.Read((ushort)(0x0100 + registers.StackPointer));

            registers.StatusRegister = (StatusRegister)statusRegister;

            return 0;
        }
    }
}