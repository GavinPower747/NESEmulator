using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0xA0, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
    [OpCode(OpCodeAddress = 0xA4, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
    [OpCode(OpCodeAddress = 0xB4, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
    [OpCode(OpCodeAddress = 0xAC, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
    [OpCode(OpCodeAddress = 0xBC, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
    public class LoadYRegisterOperation : IOperationStrategy
    {
        public string Name => "LDY";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.ReadByte(address);
            registers.Y = data;

            registers.StatusRegister.SetZeroAndNegative(registers.Y);
            return 0;
        }
    }
}