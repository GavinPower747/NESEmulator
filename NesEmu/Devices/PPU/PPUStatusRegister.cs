using NesEmu.Core;

namespace NesEmu.Devices.PPU;

public class PPUStatusRegister : ByteRegister
{
    public byte PreviousLeastSignificant { get => GetBitRange(0, 4); set => SetBitRange(0, 4, value); }
    public bool SpriteOverflow { get => GetFlag(5); set => SetFlag(5, value); }
    public bool SpriteZeroHit { get => GetFlag(6); set => SetFlag(6, value); }

    ///<summary>
    ///The vertical blank exists beyond the edge of the screen, this flag tells us if we are in screen space
    ///or the theoretical space "underneath" the screen that we are drawing to
    ///</summary>
    public bool VerticalBlankStarted { get => GetFlag(7); set => SetFlag(7, value); }

    public PPUStatusRegister(byte initialValues) : base(initialValues)
    {
    }
}