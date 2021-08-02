using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    ///<summary>
    ///NoOp, do nothing.
    ///</summary>
    ///<remarks>We are not modeling illegal opcodes they will use this operation and do nothing</remarks>
    [OpCode(OpCodeAddress = 0xEA, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class NoOpOperation : IOperationStrategy
    {
        public string Name => "NOP";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            return 0;
        }
    }
}