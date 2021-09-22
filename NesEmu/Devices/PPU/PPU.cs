using NesEmu.Core;
using System;

namespace NesEmu.Devices.PPU
{
    ///<summary>
    ///The Picture Processing Unit, think of it as a primitive graphics card
    ///</summary>
    public class PPU : IClockAware, ICPUAddressableDevice
    {
        public event SendFrame FrameReady;
        public delegate void SendFrame(byte[] frameData);
        public AddressableRange CpuRange => new AddressableRange(0x2000, 0x3FFF);

        //The nes will at any one time contain 341 cycles in a frame,
        //a cycle is roughly equivalent to a single pixel on a screen.
        //So we have a 256 wide image with 85 pixels of Overscan
        const int TotalCycles = 341;
        const int TotalScanlines = 261;
        private byte[] _frameData;
        private int _currentCycle;
        private int _currentScanline;

        private static readonly byte[] _pallet =
		{
			0x66, 0x66, 0x66, 0x00, 0x2a, 0x88, 0x14, 0x12, 0xa7, 0x3b, 0x00, 0xa4, 0x5c, 0x00, 0x7e, 0x6e,
			0x00, 0x40, 0x6c, 0x07, 0x00, 0x56, 0x1d, 0x00, 0x33, 0x35, 0x00, 0x0c, 0x48, 0x00, 0x00, 0x52,
			0x00, 0x00, 0x4f, 0x08, 0x00, 0x40, 0x4d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0xad, 0xad, 0xad, 0x15, 0x5f, 0xd9, 0x42, 0x40, 0xff, 0x75, 0x27, 0xfe, 0xa0, 0x1a, 0xcc, 0xb7,
			0x1e, 0x7b, 0xb5, 0x31, 0x20, 0x99, 0x4e, 0x00, 0x6b, 0x6d, 0x00, 0x38, 0x87, 0x00, 0x0d, 0x93,
			0x00, 0x00, 0x8f, 0x32, 0x00, 0x7c, 0x8d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0xff, 0xff, 0xff, 0x64, 0xb0, 0xff, 0x92, 0x90, 0xff, 0xc6, 0x76, 0xff, 0xf2, 0x6a, 0xff, 0xff,
			0x6e, 0xcc, 0xff, 0x81, 0x70, 0xea, 0x9e, 0x22, 0xbc, 0xbe, 0x00, 0x88, 0xd8, 0x00, 0x5c, 0xe4,
			0x30, 0x45, 0xe0, 0x82, 0x48, 0xcd, 0xde, 0x4f, 0x4f, 0x4f, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0xff, 0xff, 0xff, 0xc0, 0xdf, 0xff, 0xd3, 0xd2, 0xff, 0xe8, 0xc8, 0xff, 0xfa, 0xc2, 0xff, 0xff,
			0xc4, 0xea, 0xff, 0xcc, 0xc5, 0xf7, 0xd8, 0xa5, 0xe4, 0xe5, 0x94, 0xcf, 0xef, 0x96, 0xbd, 0xf4,
			0xab, 0xb3, 0xf3, 0xcc, 0xb5, 0xeb, 0xf2, 0xb8, 0xb8, 0xb8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
		};

        

        private readonly PPUBus _bus;

        public PPU(PPUBus bus)
        {
            _frameData = new byte[TotalCycles * TotalScanlines];
            _bus = bus;
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void Tick()
        {
            var random = new Random();
            var colour = _pallet[random.Next(0, _pallet.Length)];
            _frameData[_currentCycle * _currentScanline] = colour;

            _currentCycle++;

            if(_currentCycle >= TotalCycles)
            {
                _currentCycle = 0;
                _currentScanline++;

                if(_currentScanline >= TotalScanlines)
                {
                    _currentScanline = 0;
                    FrameReady?.Invoke(_frameData);
                }
            }
        }

        public void GetPatternTable()
        {
            for(int y = 0; y < 16; y++)
            for(int x = 0; x < 16; x++)
            {
                ushort byteOffset = (ushort)(y * 256 + x * 16);
                for(int row = 0; row < 8; row++)
                {
                    byte tileLo = _bus.ReadByte(i * 0x1000 + byteOffset + row);
                    byte tileHi = _bus.ReadByte(i * 0x1000 + byteOffset + row + 8);

                    for(int col = 0; col < 8; col++)
                    {
                        byte pixel = (byte)((tileLo & 0x01) + (tileHi & 0x01));
                        tileLo >>= 1;
                        tileHi >>= 1;
                    }
                }
            }
        }

        public byte ReadCpu(ushort address)
        {
            var mirroredAddress = address & 0x0007;
            
            switch(mirroredAddress)
            {
                case 0x0000: break;
                case 0x0001: break;
                case 0x0002: break;
                case 0x0003: break;
                case 0x0004: break;
                case 0x0005: break;
                case 0x0006: break;
                case 0x0007: break;
            }

            return 0;
        }

        public void WriteCpu(ushort address, byte data)
        {
            var mirroredAddress = address & 0x0007;
            
            switch(mirroredAddress)
            {
                case 0x0000: break;
                case 0x0001: break;
                case 0x0002: break;
                case 0x0003: break;
                case 0x0004: break;
                case 0x0005: break;
                case 0x0006: break;
                case 0x0007: break;
            }
        }
    }
}