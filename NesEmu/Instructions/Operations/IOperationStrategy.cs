using NesEmu.Core;

namespace NesEmu.Instructions.Operations
{
    public interface IOperationStrategy
    {
        ///<summary>
        ///Perform the specific operation
        ///</summary>
        ///<returns>The amount of extra cycles needed to complete the operation</returns>
        int Operate(byte data, CPURegisters registers, CPUBus bus);
    }
}