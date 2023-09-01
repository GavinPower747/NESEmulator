using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Devices.CPU;

public class StatusRegister : ByteRegister
{
    public bool Carry { get => GetFlag(0); set => SetFlag(0, value); }
    public bool Zero { get => GetFlag(1); set => SetFlag(1, value); }
    public bool InterruptDisable { get => GetFlag(2); set => SetFlag(2, value); }

    //Not used on the NES
    public bool Decimal { get => GetFlag(3); set => SetFlag(3, value); }
    public bool Break { get => GetFlag(4); set => SetFlag(4, value); }

    //Should be unused for our purposes
    //http://wiki.nesdev.com/w/index.php/Status_flags#The_B_flag
    public bool B { get => GetFlag(5); set => SetFlag(5, value); }
    public bool Overflow { get => GetFlag(6); set => SetFlag(6, value); }
    public bool Negative { get => GetFlag(7); set => SetFlag(7, value); }

    public StatusRegister(byte initialValues) : base(initialValues) { }

    public static explicit operator StatusRegister(byte b) => new(b);

    public void SetZeroAndNegative(byte value)
    {
        Zero = (value & 0x00FF) == 0;
        Negative = value.IsNegative();
    }
}