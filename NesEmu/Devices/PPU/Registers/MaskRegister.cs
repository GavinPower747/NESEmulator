using NesEmu.Core;

namespace NesEmu.Devices.PPU.Registers;

/// <summary>
/// Controls rendering of sprites and backgrounds as well as some colour effects
/// </summary>
public class MaskRegister : ByteRegister
{
    public bool IsRenderingEnabled => ShowBackground || ShowSprites;

    /// <summary>
    /// Should we render in greyscale or in a normal colour?
    /// </summary>
    public bool Greyscale { get => GetFlag(0); set => SetFlag(0, value); }

    /// <summary>
    /// Should we show the background in the leftmost 8 pixels of the screen?
    /// This is used to hide the leftmost 8 pixels of the screen when scrolling horizontally
    /// </summary>
    public bool ShowBackgroundInLeftmost8Pixels { get => GetFlag(1); set => SetFlag(1, value); }

    /// <summary>
    /// Should we show sprites in the leftmost 8 pixels of the screen?
    /// This is used to hide the leftmost 8 pixels of the screen when scrolling horizontally
    /// </summary>
    public bool ShowSpritesInLeftmost8Pixels { get => GetFlag(2); set => SetFlag(2, value); }

    public bool ShowBackground { get => GetFlag(3); set => SetFlag(3, value); }

    public bool ShowSprites { get => GetFlag(4); set => SetFlag(4, value); }

    /// <summary>
    /// Should we emphasize red?
    /// Annoyingly this is green on PAL consoles should we rename this?
    /// </summary>
    public bool EmphasizeRed { get => GetFlag(5); set => SetFlag(5, value); }

    /// <summary>
    /// Should we emphasize green?
    /// Annoyingly this is red on PAL consoles should we rename this?
    /// </summary>
    public bool EmphasizeGreen { get => GetFlag(6); set => SetFlag(6, value); }

    /// <summary>
    /// Should we emphasize blue?
    /// This is the same across all console regions
    /// </summary>
    public bool EmphasizeBlue { get => GetFlag(7); set => SetFlag(7, value); }

    public MaskRegister(byte data) : base(data) { }
}