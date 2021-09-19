using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode(OpCodeAddress = 0x46, AddressingMode = typeof(ZeroPageAddressing), Cycles = 5)]
    [OpCode(OpCodeAddress = 0x56, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 6)]
    [OpCode(OpCodeAddress = 0x4E, AddressingMode = typeof(AbsoluteAddressing), Cycles = 6)]
    [OpCode(OpCodeAddress = 0x5E, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 7)]
    public class LogicalShiftRightMemoryOperation : IOperationStrategy
    {
        public string Name => "LSR";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            byte origVal = bus.ReadByte(address);
            byte shiftedVal = (byte)(origVal / 2);

            registers.StatusRegister.Carry = origVal.GetBitValue(0);
            registers.StatusRegister.SetZeroAndNegative(shiftedVal);

            bus.Write(address, shiftedVal);

            return 0;
        }
    }

    [OpCode(OpCodeAddress = 0x4A, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class LogicalShiftRightAccumulatorOperation : IOperationStrategy
    {
        public string Name => "LSR";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            byte origVal = registers.Accumulator;
            byte shiftedVal = (byte)(origVal / 2);

            registers.StatusRegister.Carry = origVal.GetBitValue(0);
            registers.StatusRegister.SetZeroAndNegative(shiftedVal);

            registers.Accumulator = shiftedVal;

            return 0;
        }
    }
}