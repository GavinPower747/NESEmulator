namespace NesEmu.Core
{
    public interface IDevice
    {
        byte Read(byte address);
        void Write(byte address, byte data);
    }
}