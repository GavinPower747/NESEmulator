using NesEmu.Core;

namespace NesEmu.Devices.PPU.Registers;

public class ControlRegister : ByteRegister
{
    /// <summary>
    /// Which nametable should we use?
    /// 0 = 0x2000, 1 = 0x2400, 2 = 0x2800, 3 = 0x2C00
    /// </summary>
    public byte NametableAddress { get => GetBitRange(0, 1); set => SetBitRange(0, 1, value); }

    /// <summary>
    /// Should we increment the VRAM address by 1 or 32 when reading/writing?
    /// False = 1, True = 32
    /// </summary>
    public bool VRAMAddressIncrement { get => GetFlag(2); set => SetFlag(2, value); }

    /// <summary>
    /// Which sprite pattern table should we use?
    /// False = 0x0000, True = 0x1000
    /// </summary>
    public bool SpritePatternTableAddress { get => GetFlag(3); set => SetFlag(3, value); }

    /// <summary>
    /// Which background pattern table should we use?
    /// False = 0x0000, True = 0x1000
    /// </summary>
    public bool BackgroundPatternTableAddress { get => GetFlag(4); set => SetFlag(4, value); }

    /// <summary>
    /// Which size of sprite should we use?
    /// False = 8x8, True = 8x16
    /// </summary>
    public bool SpriteSize { get => GetFlag(5); set => SetFlag(5, value); }

    public bool MasterSlaveSelect { get => GetFlag(6); set => SetFlag(6, value); }    

    /// <summary>
    /// Should we generate a Non-Maskable Interrupt on VBlank?
    /// Non-Maskable interrupt is a special interrupt that cannot be disabled by the programmer
    /// VBlank is the period of time when the PPU is not drawing to the screen
    /// </summary>
    public bool GenerateNMIOnVBlank { get => GetFlag(7);  set => SetFlag(7, value); }

    public ControlRegister(byte data) : base(data)
    {

    }
}