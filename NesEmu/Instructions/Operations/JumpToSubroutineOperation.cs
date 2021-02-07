using System.Threading;
using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public class JumpToSubroutineOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.ProgramCounter--;

	        bus.Write((ushort)(0x0100 + registers.StackPointer), (byte)((registers.ProgramCounter >> 8) & 0x00FF));
	        registers.StackPointer--;
	        bus.Write((ushort)(0x0100 + registers.StackPointer), (byte)(registers.ProgramCounter & 0x00FF));
	        registers.StackPointer--;

	        registers.ProgramCounter = address;
	        return 0;
        }
    }
}