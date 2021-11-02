using NesEmu.Core;
using System;

namespace NesEmu.Devices.PPU;

///<summary>
///The Picture Processing Unit, think of it as a primitive graphics card
///</summary>
public class PPU : IClockAware, ICPUAddressableDevice, IPPUAddressableDevice
{
    public event SendFrame FrameReady;
    public delegate void SendFrame(byte[] frameData);
    public AddressableRange CpuRange => new AddressableRange(0x2000, 0x3FFF);
    public AddressableRange PPURange => new AddressableRange(0x0000, 0x3FFF);

    //The nes will at any one time contain 341 cycles in a frame,
    //a cycle is roughly equivalent to a single pixel on a screen.
    //So we have a 256 wide image with 85 pixels of Overscan
    const int TotalCycles = 341;
    const int TotalScanlines = 261;
    const int VerticalBlankStartLine = 241;
    private readonly byte[] _frameData;
    private readonly byte[] _vram;
    private int _currentCycle;
    private int _currentScanline;
    private PPUControlRegister _controlReg;
    private PPUMaskRegister _maskReg;
    private PPUStatusRegister _statusReg;
    private byte _dataBuffer;
    private ushort _ppuAddress;
    private byte[] _paletteTable;
    private bool _isSecondAddressWrite;

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
        _vram = new byte[0x2000];
        _bus = bus;
        _controlReg = new PPUControlRegister(0x00);
        _maskReg = new PPUMaskRegister(0x00);
        _statusReg = new PPUStatusRegister(0x00);
        _paletteTable = new byte[32];
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        if(_currentScanline == VerticalBlankStartLine && _currentCycle == 0)
        {
            _statusReg.VerticalBlankStarted = true;

            if(_controlReg.EnableNMI)
            {
                //At this point we will need to emit a non-maskable interrupt.
                //I need to figure out the best way to do this... eventing? 
            }
        }
        else if(_currentScanline == 1 && _currentCycle == 1)
            _statusReg.VerticalBlankStarted = false;

        var random = new Random();
        var colour = _pallet[random.Next(0, _pallet.Length)];
        _frameData[_currentCycle * _currentScanline] = colour;

        _currentCycle++;

        if (_currentCycle >= TotalCycles)
        {
            _currentCycle = 0;
            _currentScanline++;

            if (_currentScanline >= TotalScanlines)
            {
                _currentScanline = 0;
                FrameReady?.Invoke(_frameData);
            }
        }
    }

    public byte ReadCpu(ushort address)
    {
        var mirroredAddress = address & 0x0007;

        switch (mirroredAddress)
        {
            case 0x0000:
                return _controlReg;
            case 0x0001:
                return _maskReg;
            case 0x0002:
                byte data = _statusReg;

                //Reading the status register sets the vertical blank started to false
                _statusReg.VerticalBlankStarted = false;
                return data;
            case 0x0003: break;
            case 0x0004: break;
            case 0x0005: break;
            case 0x0006: 
                
                break;
            case 0x0007: 
                //Reads for some reason are delayed, when a read is requested we set it 
                //in a buffer and then return the contents of the buffer on the next read
                var bufferedData = _dataBuffer;
                var newData = _bus.ReadByte(_ppuAddress);
                _dataBuffer = newData;

                _ppuAddress++;

                //If we are looking for pattern data then just return the newest data without a buffer
                if(_ppuAddress >= 0x3f00)
                    return newData;
                else
                    return bufferedData;
        }

        return 0;
    }

    public void WriteCpu(ushort address, byte data)
    {
        var mirroredAddress = address & 0x0007;

        switch (mirroredAddress)
        {
            case 0x0000:
                _controlReg = data;
                break;
            case 0x0001:
                _maskReg = data;
                break;
            case 0x0002: break;
            case 0x0003: break;
            case 0x0004: break;
            case 0x0005: break;
            case 0x0006: 
                //The ppu address will be written by the cpu 1 byte at a time
                //store a flag saying if it is the second write and set the low
                //or high accordingly
                if(!_isSecondAddressWrite)
                {
                    _ppuAddress = (ushort)((_ppuAddress & 0x00FF) | (data << 8));
                    _isSecondAddressWrite = true;
                }
                else
                {
                    _ppuAddress = (ushort)((_ppuAddress & 0xFF00) | data);
                    _isSecondAddressWrite = false;
                }
            break;
            case 0x0007: 
                _bus.Write(_ppuAddress, data);
            break;
        }
    }

    public byte ReadPPU(ushort address)
    {
        address &= PPURange.Maximum;

        if(address >= 0x3F00)
        {
            var relevantBits = address & 0x001F;

            return _paletteTable[relevantBits - 0x0010];
        }
    }

    public void WritePPU(ushort address, byte data)
    {
        address &= PPURange.Maximum;
        throw new NotImplementedException();
    }
}