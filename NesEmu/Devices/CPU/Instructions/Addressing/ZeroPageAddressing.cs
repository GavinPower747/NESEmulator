using NesEmu.Core;

namespace NesEmu.Devices.CPU.Instructions.Addressing
{
    //adr = PEEK(arg % 256)
    public class ZeroPageAddressing : IAddressingStrategy
    {
        public (ushort address, int extraCycles) GetOperationAddress(CPURegisters registers, IBus bus)
        {
            var arg = bus.ReadByte(registers.ProgramCounter);
            ushort address = (ushort)(arg & 0x00FF);

            return (address, 0);
        }
    }
}