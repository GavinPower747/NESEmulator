namespace NesEmu.Core
{
    public class CPURegisters
    {
        public byte Accumulator;
        public byte X;
        public byte Y;
        public byte StackPointer;
        public ushort ProgramCounter; 
        public StatusRegister StatusRegister;

        private const ushort StackBaseAddress = 0x0100;

        public ushort GetStackAddress() => (ushort)(StackBaseAddress + StackPointer);
    }
}