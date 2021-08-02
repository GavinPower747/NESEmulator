using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode( OpCodeAddress = 0xA9, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
    [OpCode( OpCodeAddress = 0xA5, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
    [OpCode( OpCodeAddress = 0xB5, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xAD, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xBD, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xB9, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xA1, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
    [OpCode( OpCodeAddress = 0xB1, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
    public class LoadAccumulatorOperation : IOperationStrategy
    {
        public string Name => "LDA";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.ReadByte(address);
            registers.Accumulator = data;

            registers.StatusRegister.SetZeroAndNegative(registers.Accumulator);
            return 0;
        }
    }
}