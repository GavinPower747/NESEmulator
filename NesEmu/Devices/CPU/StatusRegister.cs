using NesEmu.Extensions;

namespace NesEmu.Devices.CPU
{
    public class StatusRegister
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

        private byte _flags { get; set; }

        public StatusRegister(byte initialValues)
        {
            _flags = initialValues;
        }

        public static implicit operator byte(StatusRegister register) => register._flags;
        public static explicit operator StatusRegister(byte b) => new StatusRegister(b);

        public void SetZeroAndNegative(byte value)
        {
            Zero = (value & 0x00FF) == 0;
            Negative = value.IsNegative();
        }

        private bool GetFlag(int index)
        {
            var mask = (byte)(1 << index);

            return (_flags & mask) != 0;
        }

        private void SetFlag(int index, bool value)
        {
            byte mask = (byte)(1 << index);

            if(value)
                _flags |= mask;
            else
                _flags &= (byte)~mask;
        }
    }
}