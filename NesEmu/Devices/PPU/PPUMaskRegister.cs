using NesEmu.Core;

namespace NesEmu.Devices.PPU
{
    public class PPUMaskRegister : ByteRegister
    {
        public bool Grayscale { get => GetFlag(0); set => SetFlag(0, value); }
        public bool RenderBackgroundLeft { get => GetFlag(1); set => SetFlag(1, value); }
        public bool RenderSpritesLeft { get => GetFlag(2); set => SetFlag(2, value); }
        public bool RenderBackground { get => GetFlag(3); set => SetFlag(3, value); }
        public bool RenderSprites { get => GetFlag(4); set => SetFlag(4, value); }
        public bool EnhanceRed { get => GetFlag(5); set => SetFlag(5, value); }
        public bool EnhanceGreen { get => GetFlag(6); set => SetFlag(6, value); }
        public bool EnhanceBlue { get => GetFlag(7); set => SetFlag(7, value); }

        public PPUMaskRegister(byte initialValues) : base(initialValues)
        {
        }
    }
}