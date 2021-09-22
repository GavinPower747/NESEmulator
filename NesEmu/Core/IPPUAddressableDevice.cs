namespace NesEmu.Core
{
    public interface IPPUAddressableDevice : IAddressableDevice
    {
        AddressableRange PPURange { get; }
        byte ReadPPU(ushort address);
        void WritePPU(ushort address, byte data);
    }
}