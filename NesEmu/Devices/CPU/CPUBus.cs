using System;
using NesEmu.Core;

namespace NesEmu.Devices.CPU
{
    public class CPUBus : IBus
    {
        private CPU _cpu;
        private IDevice[] _connectedDevices;
        private byte[] _ram; //Test Purposes please delete after use. Write some actual devices

        public CPUBus(CPU cpu)
        {
            _cpu = cpu;
            _connectedDevices = Array.Empty<IDevice>();
            _cpu.ConnectBus(this);

            _ram = new byte[64 * 1024]; //64kb of RAM
        }

        public byte ReadByte(ushort address)
        {
            if(address >= 0x0000 && address <= 0xFFFF)
                return _ram[address];

            return 0;
        }

        public ushort ReadWord(ushort address)
        {
            if(address >= 0x0000 && address <= 0xFFFF)
            {
                var lo = (ushort)_ram[address];
                var hi = (ushort)_ram[address + 1];

                return (ushort)(hi << 8 | lo);
            }

            return 0;    
        }

        public void Write(ushort address, byte data)
        {
            if(address >= 0x0000 && address <= 0xFFFF)
                 _ram[address] = data;
        }
    }
}