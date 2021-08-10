using NesEmu.Core;

namespace NesEmu.Devices
{
    public class Ram : IAddressableDevice
    {
        public AddressableRange AddressableRange => new AddressableRange(0x00, 0xFFFF);
        private byte[] _data;
        private ushort _mirrorMask = 2047;

        public Ram()
        {
            _data = new byte[1024 * 64]; //2KB
        }

        public byte Read(ushort address)
        {
            return _data[address];
        }

        public void Write(ushort address, byte data)
        {
            _data[address] = data;
        }
    }
}