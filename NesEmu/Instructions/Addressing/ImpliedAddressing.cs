using NesEmu.Core;

namespace NesEmu.Instructions.Addressing
{
    ///<summary>
    ///Instruction requires no additional data.
    ///</summary>
    public class ImpliedAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            return (registers.ProgramCounter, 0);
        }
    }
}