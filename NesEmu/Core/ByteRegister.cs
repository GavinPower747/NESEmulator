using System;

namespace NesEmu.Core;

///<summary>
/// Represents a register that is 8 bits wide
/// Has a series of convenience methods for getting and setting individual or ranges of bits
/// Can also be used interchangeably with a byte
///</summary>
public abstract class ByteRegister
{
    private byte _backingByte;

    protected ByteRegister(byte initialValues)
    {
        _backingByte = initialValues;
    }

    protected bool GetFlag(byte index)
    {
        if(index < 0 || index > 7)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 7");

        var mask = (byte)(1 << index);

        return (_backingByte & mask) != 0;
    }

    protected void SetFlag(byte index, bool value)
    {
        if (index < 0 || index > 7)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 7");

        byte mask = (byte)(1 << index);

        if (value)
            _backingByte |= mask;
        else
            _backingByte &= (byte)~mask;
    }

    ///<summary>
    ///Isolates a range of bits and returns them in a new byte
    ///</summary>
    ///<remarks>Overly verbose and commented as this took me ages to come up with and wrap my head around</remarks>
    protected byte GetBitRange(int startIndex, int endIndex)
    {
        if(startIndex > endIndex)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Start index must be less than or equal to end index");
        
        if(startIndex < 0 || startIndex > 7)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Start index must be between 0 and 7");
        
        if(endIndex < 0 || endIndex > 7)
            throw new ArgumentOutOfRangeException(nameof(endIndex), "End index must be between 0 and 7");

        byte count = (byte)((endIndex - startIndex) + 1);
        byte shifted = (byte)(_backingByte >> startIndex); //shift the desired bits over to the far right of the byte

        //if we take the count and subtract 1 we will have a perfect mask to extract our shifted bits
        //e.g. 1 << 3 = 00001000; 00001000 - 1 = 00000111
        byte mask = (byte)((1 << count) - 1);

        //extract the bits using the AND operator
        return (byte)(shifted & mask);
    }

    ///<summary>
    ///Takes the first x bits of the supplied byte and sets them over the backing byte
    ///</summary>
    ///<remarks>Overly verbose and commented as this took me ages to come up with and wrap my head around</remarks>
    protected void SetBitRange(int startIndex, int endIndex, byte data)
    {
        if(startIndex > endIndex)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Start index must be less than or equal to end index");
        
        if(startIndex < 0 || startIndex > 7)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Start index must be between 0 and 7");
        
        if(endIndex < 0 || endIndex > 7)
            throw new ArgumentOutOfRangeException(nameof(endIndex), "End index must be between 0 and 7");

        byte count = (byte)((endIndex - startIndex) + 1);
        byte mask = (byte)((1 << count) - 1);

        //AND the backing byte with the inverse of the mask, this zeros out the range we're setting while preserving the rest of the byte
        byte zeroedOut = (byte)(_backingByte & ~(mask << startIndex));
        byte newBits = (byte)(data & mask); //Only take the right amount of bits

        //Shift the new bits to the correct location and set them on the zeroed version of the original value
        _backingByte = (byte)(zeroedOut | (newBits << startIndex));
    }

    public static implicit operator byte(ByteRegister register) => register._backingByte;
}