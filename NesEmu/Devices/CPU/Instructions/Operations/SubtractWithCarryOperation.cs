using System;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0xE9, AddressingMode = typeof(ImmediateAddressing), Cycles = 2)]
    [OpCode(OpCodeAddress = 0xE5, AddressingMode = typeof(ZeroPageAddressing), Cycles = 3)]
    [OpCode(OpCodeAddress = 0xF5, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 4)]
    [OpCode(OpCodeAddress = 0xED, AddressingMode = typeof(AbsoluteAddressing), Cycles = 4)]
    [OpCode(OpCodeAddress = 0xFD, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 4)]
    [OpCode(OpCodeAddress = 0xF9, AddressingMode = typeof(AbsoluteYOffsetAddressing), Cycles = 4)]
    [OpCode(OpCodeAddress = 0xE1, AddressingMode = typeof(IndirectXAddressing), Cycles = 6)]
    [OpCode(OpCodeAddress = 0xF1, AddressingMode = typeof(IndirectYAddressing), Cycles = 5)]
    public class SubtractWithCarryOperation : IOperationStrategy
    {
        public string Name => "SBC";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var fetched = bus.ReadByte(address);

            ushort value = (ushort)(fetched ^ 0x00FF);
            byte carryValue = registers.StatusRegister.Carry ? (byte)1 : (byte)0;
            
            byte subtracted = (byte)(registers.Accumulator + value + carryValue);
            registers.StatusRegister.Carry = subtracted > 255;
            registers.StatusRegister.SetZeroAndNegative(subtracted);
            registers.StatusRegister.Overflow = Convert.ToBoolean(((~(registers.Accumulator ^ value) & (registers.Accumulator ^ value)) & 0x0080));

            registers.Accumulator = (byte)(subtracted & 0x00FF);
            return 1;
        }
    }
}