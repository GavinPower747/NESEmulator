using NesEmu.Core;

namespace NesEmu.Extensions
{
    public static class BitwiseExtensions
    {
        public static void SetFlag(this StatusRegister flags, StatusRegister flag, bool setting)
        {
            if(setting)
                flags |= flag;
            else
                flags &= ~flag;
        }
    }
}