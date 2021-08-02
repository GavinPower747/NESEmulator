using System;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions.Addressing;

namespace NesEmu.Devices.CPU.Instructions.Operations
{
    [OpCode( OpCodeAddress = 0x00, AddressingMode = typeof(ImpliedAddressing), Cycles = 7)]
    public class BreakOperation : IOperationStrategy
    {
        public string Name => "BRK";

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            registers.ProgramCounter++;
            registers.StatusRegister.InterruptDisable = true;
            
            bus.Write(registers.GetStackAddress(), (byte)((registers.ProgramCounter >> 8) & 0x00FF));
            registers.StackPointer--;

            bus.Write(registers.GetStackAddress(), (byte)(registers.ProgramCounter & 0x00FF));
            registers.StackPointer--;

            registers.StatusRegister.Break = true;
            bus.Write(registers.GetStackAddress(), registers.StatusRegister);
            registers.StackPointer--;
            registers.StatusRegister.Break = false;

            registers.ProgramCounter = (ushort)(bus.ReadByte(0xFFFE) | (bus.ReadByte(0xFFFF) << 8));

            return 0;
        }
    }
}