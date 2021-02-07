using NesEmu.Core;

namespace NesEmu.Extensions
{
    public static class BitwiseExtensions
    {
        //TODO: Find out why this extension doesn't work but just going |= inline does
        public static StatusRegister SetFlag(this StatusRegister flags, StatusRegister flag, bool setting)
        {
            if(setting)
                return flags |= flag;
            else
                return flags &= ~flag;
        }
    }
}