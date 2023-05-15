namespace NesEmu.Core;

public interface IBus
{
    byte ReadByte(ushort address);
    ushort ReadWord(ushort address);
    void Write(ushort address, byte data);
    void ConnectDevice(IAddressableDevice device);
}