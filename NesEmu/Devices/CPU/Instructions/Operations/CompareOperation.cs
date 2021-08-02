using System;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    public abstract class CompareOperation : IOperationStrategy
    {
        public abstract string Name { get; }

        private Func<CPURegisters, byte> _selector;

        public abstract int Operate(ushort address, CPURegisters registers, IBus bus);

        protected int PerformOperation(byte registerVal, ushort address, CPURegisters registers, IBus bus)
        {
            ushort memoryVal = bus.ReadByte(address);
            ushort comparedVal = (ushort)(registerVal - memoryVal);

            registers.StatusRegister.Carry = comparedVal >= 0;
            registers.StatusRegister.Zero = comparedVal == 0;
            registers.StatusRegister.Negative = (comparedVal & 1 << 7) > 0;

            return 0;
        }
    }

    [OpCode( OpCodeAddress = 0xC9, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
    [OpCode( OpCodeAddress = 0xC5, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
    [OpCode( OpCodeAddress = 0xD5, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xCD, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xDD, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xD9, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
    [OpCode( OpCodeAddress = 0xC1, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
    [OpCode( OpCodeAddress = 0xD1, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
    public class CompareAccumulator : CompareOperation
    {
        public override string Name => "CMP";

        public override int Operate(ushort address, CPURegisters registers, IBus bus)
            => PerformOperation(registers.Accumulator, address, registers, bus);
    }

    [OpCode( OpCodeAddress = 0xE0, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
    [OpCode( OpCodeAddress = 0xE4, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
    [OpCode( OpCodeAddress = 0xEC, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
    public class CompareXRegister : CompareOperation
    {
        public override string Name => "CPX";

        public override int Operate(ushort address, CPURegisters registers, IBus bus)
            => PerformOperation(registers.X, address, registers, bus);
    }

    [OpCode( OpCodeAddress = 0xC0, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
    [OpCode( OpCodeAddress = 0xC4, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
    [OpCode( OpCodeAddress = 0xCC, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
    public class CompareYRegister : CompareOperation
    {
        public override string Name => "CPY";

        public override int Operate(ushort address, CPURegisters registers, IBus bus)
            => PerformOperation(registers.Y, address, registers, bus);
    }
}