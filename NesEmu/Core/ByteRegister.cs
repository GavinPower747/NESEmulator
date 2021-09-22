namespace NesEmu.Core
{
    public abstract class ByteRegister
    {
        private byte _backingByte;

        protected ByteRegister(byte initialValues)
        {
            _backingByte = initialValues;
        }

        protected bool GetFlag(int index)
        {
            var mask = (byte)(1 << index);

            return (_backingByte & mask) != 0;
        }

        protected void SetFlag(int index, bool value)
        {
            byte mask = (byte)(1 << index);

            if(value)
                _backingByte |= mask;
            else
                _backingByte &= (byte)~mask;
        }

        public static implicit operator byte(ByteRegister register) => register._backingByte;
    }
}