using NesEmu.Core;

namespace NesEmu.Devices.PPU.Registers;

/// <summary>
/// Reflects the state of various functions inside the PPU 
/// </summary>
public class StatusRegister : ByteRegister
{
    /// <summary>
    /// Returns statle PPU data from the last read
    /// </summary>
    public byte OpenBusContent { get => GetBitRange(0, 4); set => SetBitRange(0, 4, value); }

    /// <summary>
    /// Do more than 8 sprites appear on a scanline?
    /// </summary>
    public bool SpriteOverflow { get => GetFlag(5); set => SetFlag(5, value); }

    /// <summary>
    /// Does a pixel of sprite 0 overlap a pixel of the background?
    /// </summary>
    public bool SpriteZeroHit { get => GetFlag(6); set => SetFlag(6, value); }

    /// <summary>
    /// Is the PPU currently in VBlank?
    /// </summary>
    /// <remarks>
    /// Vertical Blank is a theoretical region of the television below the visible screen
    /// It is used to give the CPU time to do work without affecting the display
    /// </remarks>
    public bool IsInVBlank { get => GetFlag(7); set => SetFlag(7, value); }

    public StatusRegister(byte data) : base(data) { }
}