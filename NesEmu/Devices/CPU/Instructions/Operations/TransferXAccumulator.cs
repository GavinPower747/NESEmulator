using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0x8A, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class TransferXAccumulator : IOperationStrategy
    {
        public string Name => "TXA";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Accumulator = registers.X;

            return 0;
        }
    }
}