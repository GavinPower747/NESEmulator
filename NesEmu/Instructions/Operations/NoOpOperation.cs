using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    ///<summary>
    ///NoOp, do nothing.
    ///</summary>
    ///<remarks>We are not modeling illegal opcodes they will use this operation and do nothing</remarks>
    public class NoOpOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            return 0;
        }
    }
}