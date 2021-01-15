namespace NesEmu.Core
{
    public class CPURegisters
    {
        public byte Accumulator;
        public byte XRegister;
        public byte YRegister;
        public byte StackPointer;
        public ushort ProgramCounter; 
        public StatusRegister StatusRegister;
    }
}