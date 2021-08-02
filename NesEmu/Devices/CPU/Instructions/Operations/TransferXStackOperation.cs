using System.Reflection.Emit;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0x9A, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class TransferXStackOperation : IOperationStrategy
    {
        public string Name => "TXS";
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StackPointer = registers.X;

            return 0;
        }
    }
}