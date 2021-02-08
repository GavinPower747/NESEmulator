using System;

namespace NesEmu.Core
{
    [Flags]
    public enum StatusRegister : byte
    {
        Carry = (1 << 0),
        Zero = (1 << 1),
        InterruptDisable = (1 << 2),

        //Not used on the NES
        Decimal = (1 << 3),
        Break = (1 << 4),
        
        //Should be unused for our purposes
        //http://wiki.nesdev.com/w/index.php/Status_flags#The_B_flag
        B = (1 << 5),
        Overflow = (1 << 6),
        Negative = (1 << 7)
    }
}