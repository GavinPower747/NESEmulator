using NesEmu.Devices;
using NesEmu.Devices.CPU;
using NesEmu.Devices.Cartridge;
using NesEmu.Devices.PPU;

namespace NesEmu.Core;


///<summary>
///The main public API to access console functions
///</summary>
public class NintendoEntertainmentSystem
{
    public readonly Disassembler Disassembler;
    
    private readonly PPU PPU;
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
        PPU = new PPU(_ppuBus);
        _ram = new Ram();
        Disassembler = new Disassembler(_cpuBus, _ppuBus, _processor);

        _cpuBus.ConnectDevice(_ram);
        _cpuBus.ConnectDevice(PPU);
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

    public void SaveState()
    {
        //TODO
    }

    public void LoadState()
    {
        //Todo
    }
}