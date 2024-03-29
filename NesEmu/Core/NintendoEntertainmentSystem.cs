using NesEmu.Devices;
using NesEmu.Devices.CPU;
using NesEmu.Devices.Cartridge;
using NesEmu.Devices.PPU;
using System;

namespace NesEmu.Core;


///<summary>
///The main public API to access console functions
///</summary>
public class NintendoEntertainmentSystem
{
    public readonly Disassembler Disassembler;
    public event EventHandler<byte[]> FrameReady;

    private readonly PPU _ppu;
    private readonly Cpu _processor;
    private readonly CpuBus _cpuBus;
    private readonly PPUBus _ppuBus;
    private readonly Ram _ram;
    private Cartridge _cartridge;

    public NintendoEntertainmentSystem()
    {
        _processor = new Cpu();
        _cpuBus = new CpuBus(_processor);
        _ppuBus = new PPUBus();
        _ppu = new PPU(_ppuBus);
        _ram = new Ram();
        Disassembler = new Disassembler(_cpuBus, _ppuBus, _processor);

        _cpuBus.ConnectDevice(_ram);
        _cpuBus.ConnectDevice(_ppu);

        _ppu.FrameReady += (s, e) => FrameReady?.Invoke(s, e);
    }

    public void Reset()
    {
        _processor.Reset();
    }

    public void Interrupt()
    {
        _processor.Interrupt();
    }

    public void NonMaskableInterrupt()
    {
        _processor.NonMaskableInterrupt();
    }

    public void LoadCartridge(string pathToRom)
    {
        _cartridge = new Cartridge(pathToRom);
        _cartridge.Load();

        _cpuBus.ConnectDevice(_cartridge);
        _ppuBus.ConnectDevice(_cartridge);
    }

    public void Step()
    {
        _ppu.Tick();
    }

    public void SaveState()
    {
        //TODO
    }

    public void LoadState()
    {
        //Todo
    }
}
