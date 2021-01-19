using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class BreakOperation : IOperationStrategy
    {
        public int Operate(byte data, CPURegisters registers, IBus bus)
        {
            registers.ProgramCounter++;
            registers.StatusRegister.SetFlag(StatusRegister.InterruptDisable, true);
            
            bus.Write((ushort)(0x0100 + registers.StackPointer), (byte)((registers.ProgramCounter >> 8) & 0x00FF));
            registers.StackPointer--;

            bus.Write((ushort)(0x0100 + registers.StackPointer), (byte)(registers.ProgramCounter & 0x00FF));
            registers.StackPointer--;

            registers.StatusRegister.SetFlag(StatusRegister.Break, true);
            bus.Write((ushort)(0x0100 + registers.StackPointer), Convert.ToByte(registers.StatusRegister));
            registers.StackPointer--;
            registers.StatusRegister.SetFlag(StatusRegister.Break, false);

            registers.ProgramCounter = (ushort)(bus.Read(0xFFFE) | (bus.Read(0xFFFF) << 8));

            return 0;
        }
    }
}