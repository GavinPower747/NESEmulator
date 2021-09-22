namespace NesEmu.Core
{
    public interface ICPUAddressableDevice : IAddressableDevice
    {
        AddressableRange CpuRange { get; }
        byte ReadCpu(ushort address);
        void WriteCpu(ushort address, byte data);
    }
}