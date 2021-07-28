namespace NesEmu.Extensions
{
    public static class BitwiseExtensions
    {
        ///<summary>
        ///Determine if the value is negative by examining the most significant bit
        ///</summary>
        public static bool IsNegative(this byte val)
        {
            byte negativeMask = 1 << 7;

            return (val & negativeMask) == 1;
        }

        ///<summary>
        ///Determine if a particular bit in a byte is active
        ///</summary>
        public static bool GetBitValue(this byte val, int bitNumber)
        {
            byte bitMask = (byte)(1 << (bitNumber - 1));

            return (val & bitMask) > 0;
        }

        ///<summary>
        ///Determine if two shorts are on the same memory page by comparing the hi byte
        ///</summary>
        public static bool IsOnSamePageAs(this ushort val, ushort other)
        {
            return (val & 0xFF00) == (other & 0xFF00);
        }
    }
}