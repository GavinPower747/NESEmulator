using NesEmu.Core;

namespace NesEmu.Devices.PPU;

public class PPUControlRegister : ByteRegister
{
    public bool NametableX { get => GetFlag(0); set => SetFlag(0, value); }
    public bool NametableY { get => GetFlag(1); set => SetFlag(1, value); }
    public bool IncrementMode { get => GetFlag(2); set => SetFlag(2, value); }
    public bool PatternSprite { get => GetFlag(3); set => SetFlag(3, value); }
    public bool PatternBackground { get => GetFlag(4); set => SetFlag(4, value); }
    public bool SpriteSize { get => GetFlag(5); set => SetFlag(5, value); }
    public bool SlaveMode { get => GetFlag(6); set => SetFlag(6, value); }
    public bool EnableNMI { get => GetFlag(7); set => SetFlag(7, value); }

    public PPUControlRegister(byte initialValues) : base(initialValues)
    {
    }

    public static implicit operator PPUControlRegister(byte backingByte) => new PPUControlRegister(backingByte);
}
