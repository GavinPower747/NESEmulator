using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0x78, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class SetInteruptDisableOperation : IOperationStrategy
    {
        public string Name => "SEI";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.StatusRegister.InterruptDisable = true;

            return 0;
        }
    }
}