using NesEmu.Core;

namespace NesEmu.Extensions
{
    public static class BitwiseExtensions
    {
        //TODO: Find out why this extension doesn't work but just going |= inline does
        public static void SetFlag(this StatusRegister flags, StatusRegister flag, bool setting)
        {
            if(setting)
                flags |= flag;
            else
                flags &= ~flag;
        }
    }
}