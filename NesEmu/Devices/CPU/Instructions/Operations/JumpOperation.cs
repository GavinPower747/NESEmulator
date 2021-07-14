using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public class JumpOperation : IOperationStrategy
    {
        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var jumpLocation = bus.ReadByte(address);

            registers.ProgramCounter = jumpLocation;

            return 0;
        }
    }
}