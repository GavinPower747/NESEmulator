using System.Reflection.Emit;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0xA8, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class TransferAccumulatorY : IOperationStrategy
    {
        public string Name => "TAY";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Y = registers.Accumulator;
            registers.StatusRegister.Overflow = !registers.StatusRegister.Overflow;
            return 0;
        }
    }
}