namespace NesEmu.Core
{
    public interface IBus
    {
        byte Read(ushort address);
        void Write(ushort address, byte data);
    } 
}