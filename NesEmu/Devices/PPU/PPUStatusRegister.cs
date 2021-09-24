using NesEmu.Core;

namespace NesEmu.Devices.PPU
{
    public class PPUStatusRegister : ByteRegister
    {
        public byte PreviousLeastSignificant { get => GetBitRange(0, 4); set => SetBitRange(0, 4, value); }
        public bool SpriteOverflow { get => GetFlag(5); set => SetFlag(5, value); }
        public bool SpriteZeroHit { get => GetFlag(6); set => SetFlag(6, value); }
        public bool VerticalBlankStarted { get => GetFlag(7); set => SetFlag(7, value); }

        public PPUStatusRegister(byte initialValues) : base(initialValues)
        {
        }
    }
}