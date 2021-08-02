using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode( OpCodeAddress = 0xC6, AddressingMode = typeof(ZeroPageAddressing), Cycles = 5)]
    [OpCode( OpCodeAddress = 0xD6, AddressingMode = typeof(ZeroPageXOffsetAddressing), Cycles = 6)]
    [OpCode( OpCodeAddress = 0xCE, AddressingMode = typeof(AbsoluteAddressing), Cycles = 6)]
    [OpCode( OpCodeAddress = 0xDE, AddressingMode = typeof(AbsoluteXOffsetAddressing), Cycles = 7)]
    public class DecrementMemoryOperation : IOperationStrategy
    {
        public string Name => "DEC";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            var data = bus.ReadByte(address);
            byte newValue = (byte)(data - 1);

            bus.Write(address, newValue);

            registers.StatusRegister.SetZeroAndNegative(newValue);

            return 0;
        }
    }

    [OpCode( OpCodeAddress = 0xCA, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class DecrementXRegisterOperation : IOperationStrategy
    {
        public string Name => "DEX";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.X -= 1;

            registers.StatusRegister.SetZeroAndNegative(registers.X);
            return 0;
        }
    }

    [OpCode( OpCodeAddress = 0x88, AddressingMode = typeof(ImpliedAddressing), Cycles = 2)]
    public class DecrementYRegisterOperation : IOperationStrategy
    {
        public string Name => "DEY";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.Y -= 1;

            registers.StatusRegister.SetZeroAndNegative(registers.Y);

            return 0;
        }
    }
}