using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public interface IOperationStrategy
    {
        ///<summary>
        ///Perform the specific operation
        ///</summary>
        ///<returns>The amount of extra cycles needed to complete the operation</returns>
        int Operate(ushort address, CPURegisters registers, IBus bus);
    }
}