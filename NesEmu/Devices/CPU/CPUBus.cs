using System.Collections.Generic;
using System.Linq;
using NesEmu.Core;

namespace NesEmu.Devices.CPU
{
    public class CPUBus : IBus
    {
        private CPU _cpu;
        private List<IAddressableDevice> _connectedDevices;

        public CPUBus(CPU cpu)
        {
            _cpu = cpu;
            _connectedDevices = new List<IAddressableDevice>();
            _cpu.ConnectBus(this);
        }

        public byte ReadByte(ushort address)
        {
            var device = _connectedDevices.FirstOrDefault(x => x.AddressableRange.ContainsAddress(address));

            if(device is not null)
            {
                return device.Read(address);
            }

            return 0;
        }

        public ushort ReadWord(ushort address)
        {
            var device = _connectedDevices.FirstOrDefault(x => x.AddressableRange.ContainsAddress(address));

            if(device is not null)
            {
                var lo = (ushort)device.Read(address);
                var hi = (ushort)device.Read((ushort)(address + 1));

                return (ushort)(hi << 8 | lo);
            }

            return 0;    
        }

        public void Write(ushort address, byte data)
        {
            var device = _connectedDevices.FirstOrDefault(x => x.AddressableRange.ContainsAddress(address));

            if(device is not null)
            {
                device.Write(address, data);
            }
        }

        public void ConnectDevice(IAddressableDevice device)
        {
            //Do validation to make sure that ranges aren't overlapping
            _connectedDevices.Add(device);
        }
    }
}