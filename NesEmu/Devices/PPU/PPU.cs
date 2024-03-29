using System;
using NesEmu.Core;
using NesEmu.Devices.PPU.Registers;

namespace NesEmu.Devices.PPU;

///<summary>
///The Picture Processing Unit, think of it as a primitive graphics card
///</summary>
public class PPU : IClockAware, ICPUAddressableDevice, IPPUAddressableDevice
{
    public AddressableRange CpuRange => throw new NotImplementedException();
    public AddressableRange PPURange => throw new NotImplementedException();

    // Clock
    private int _currentScanline = 0;
    private int _currentCycle = 0;
    private PPURenderingStage _currentRenderingStage = PPURenderingStage.PreRender;
    private ulong _tileData = 0;

    // Frames
    private int[][] _foregroundPixels;
    private int[][] _backgroundPixels;

    // Registers
    private StatusRegister _statusRegister;
    private MaskRegister _maskRegister;
    private ControlRegister _controlRegister;

    // Dependencies
    private readonly IBus _ppuBus;

    // Constants
    private const int TotalScanlines = 262;
    private const int CyclesPerLine = 341;
    private const int VisibleScanlines = 240;

    // Palette
    private byte[] _palleteData;

    public event EventHandler<byte[]> FrameReady;
    private byte[] _frameBuffer = new byte[TotalScanlines * CyclesPerLine];

    public PPU(IBus ppuBus)
    {
        _statusRegister = new StatusRegister(0x00);
        _maskRegister = new MaskRegister(0x00);
        _controlRegister = new ControlRegister(0x00);

        _foregroundPixels = new int[VisibleScanlines][];
        _backgroundPixels = new int[VisibleScanlines][];

        _palleteData = new byte[32];

        _ppuBus = ppuBus;
    }

    public void Tick()
    {
        byte white = 0xFF;
        byte black = 0x00;

        _currentCycle++;

        if (_currentCycle >= CyclesPerLine)
        {
            _currentCycle = 0;
            _currentScanline++;

            if (_currentScanline >= TotalScanlines)
            {
                _currentScanline = 0;
                FrameReady?.Invoke(this, _frameBuffer);
                _frameBuffer = new byte[TotalScanlines * CyclesPerLine];
            }
        }

        var random = new Random().Next() % 2;

        _frameBuffer[_currentScanline * CyclesPerLine + _currentCycle] = random switch
        {
            0 => black,
            _ => white
        };
    }

    private void Render()
    {
        _currentCycle++;

        if (_currentCycle >= CyclesPerLine)
        {
            _currentCycle = 0;
            _currentScanline++;

            if (_currentScanline >= TotalScanlines)
            {
                _currentScanline = 0;

                // Dispatch a frame ready event update the screen after every scanline
            }
        }

        SetRenderingStage();

        if (_currentRenderingStage == PPURenderingStage.Visible)
        {
            RenderPixel();

            switch (_currentCycle % 8)
            {
                case 1:
                    // Fetch name table byte
                    break;
                case 3:
                    // Fetch attribute table byte
                    break;
                case 5:
                    // Fetch low tile byte
                    break;
                case 7:
                    // Fetch high tile byte
                    break;
                case 0:
                    // Increment scroll x
                    break;
            }
        }
        else if (_currentRenderingStage == PPURenderingStage.PreRender)
        {
            if (_currentCycle == 1)
            {
                _statusRegister.IsInVBlank = false;
                _statusRegister.SpriteZeroHit = false;
                _statusRegister.SpriteOverflow = false;
            }
            else if (_currentCycle == 280)
            {
                // Copy Y to V
            }
            else if (_currentCycle == 304)
            {
                // Copy X to V
            }
        }


    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public byte ReadCpu(ushort address)
    {
        throw new NotImplementedException();
    }

    public byte ReadPPU(ushort address)
    {
        throw new NotImplementedException();
    }

    public void WriteCpu(ushort address, byte data)
    {
        throw new NotImplementedException();
    }

    public void WritePPU(ushort address, byte data)
    {
        if (address >= 0x3F00 && address <= 0x3FFF)
        {
            _palleteData[address & 0x1F] = data;
        }
    }

    private void RenderPixel()
    {
        int x = _currentCycle - 1;
        int y = _currentScanline;

        int backgroundPixel = _maskRegister.ShowBackground ? FetchTileData() : 0;

        if (x < 8 && !_maskRegister.ShowBackgroundInLeftmost8Pixels)
        {
            backgroundPixel = 0;
        }

        byte colour;

        if (backgroundPixel == 0)
        {
            colour = 0;
        }
        else
        {
            int palette = (backgroundPixel & 0x3F) >> 2;
            int pixel = backgroundPixel & 0x03;

            colour = _ppuBus.ReadByte((ushort)(0x3F00 + (palette * 4) + pixel));
        }


        _backgroundPixels[y][x] = colour;
    }

    private void SetRenderingStage()
    {
        _currentRenderingStage = _currentScanline switch
        {
            -1 => PPURenderingStage.PreRender,
            < VisibleScanlines => PPURenderingStage.Visible,
            240 => PPURenderingStage.PostRender,
            >= 241 => PPURenderingStage.VerticalBlank
        };
    }

    private byte FetchTileData()
    {
        throw new NotImplementedException();
    }
}
