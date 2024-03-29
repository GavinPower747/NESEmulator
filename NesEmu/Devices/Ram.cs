using NesEmu.Core;

namespace NesEmu.Devices;

public class Ram : ICPUAddressableDevice
{
    //You'll notice that RAM covers an 8KB range despite there only being 2KB of actual memory.
    //This is because any addresses over the 2KB range are mirrored back into the original range
    //i.e. if address < 2048 then read(address), if address > 2048 then read(address - 2048), if address > 4096 then read(address - 4096) etc.
    public AddressableRange CpuRange => new (0x00, 0x1FFF);
    private readonly byte[] _data;
    private const ushort _mirrorMask = 2047;

    public Ram()
    {
        _data = new byte[1024 * 2]; //2KB
    }

    public byte ReadCpu(ushort address)
    {
        var readAddress = (ushort)(address & _mirrorMask);
        return _data[readAddress];
    }

    public void WriteCpu(ushort address, byte data)
    {
        var writeAddress = (ushort)(address & _mirrorMask);
        _data[writeAddress] = data;
    }
}