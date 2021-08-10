namespace NesEmu.Core
{
    public interface IDevice
    {
        byte Read(ushort address);
        void Write(ushort address, byte data);
    }
}