namespace NesEmu.Core
{
    public interface IAddressableDevice : IDevice
    {
        AddressableRange AddressableRange { get; }
    }
}