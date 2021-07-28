using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class BranchOperation : IOperationStrategy
    {
        private Func<CPURegisters, bool> _predicate; 

        public BranchOperation(Func<CPURegisters, bool> predicate)
        {
            _predicate = predicate;
        }

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            if(!_predicate(registers))
                return 0;

            var initialProgramCounter = registers.ProgramCounter;
            registers.ProgramCounter = address;

            if(initialProgramCounter.IsOnSamePageAs(registers.ProgramCounter))
                return 1;
            else
                return 2;
        }
    }
}