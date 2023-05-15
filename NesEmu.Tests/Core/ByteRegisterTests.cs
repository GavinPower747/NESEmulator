using System;
using NesEmu.Core;
using NUnit.Framework;

namespace NesEmu.Tests.Core;

[TestFixture]
public class ByteRegisterTests
{
    [Test]
    public void Can_Set_Flag()
    {
        TestRegister testRegister = new();

        testRegister.FourthBit = true;

        Assert.That(testRegister.FourthBit, Is.True);
    }

    [Test]
    public void Can_Get_Flag()
    {
        byte testByte = 0b000010000;
        TestRegister testRegister = new(testByte);

        var bit = testRegister.FourthBit;

        Assert.That(bit, Is.EqualTo((testByte & (1 << 4)) == 0));
    }

    [Test]
    public void Can_Set_BitRange()
    {
        TestRegister testRegister = new(0b11111111);

        testRegister.MultiByte = 0b101;

        Assert.That(testRegister.MultiByte, Is.EqualTo(0b101));
    }

    [Test]
    public void Set_DoesntChange_OtherBits()
    {
        TestRegister register = new(0b11111111);

        register.MultiByte = 0b000;
        byte backingByte = register;

        Assert.That(backingByte, Is.EqualTo(0b11110001));
    }

    [Test]
    public void Set_DoesntChange_OtherBits_When_BitIsOversized()
    {
        TestRegister register = new(0b11111111);

        register.MultiByte = 0b00001011;
        byte backingByte = register;

        Assert.That(backingByte, Is.EqualTo(0b11110111));
    }

    [Test]
    public void Can_Get_BitRange()
    {
        byte testByte = 0b000001010;
        TestRegister testRegister = new(testByte);

        Assert.That(testRegister.MultiByte, Is.EqualTo(0b101));
    }


    public class TestRegister : ByteRegister
    {
        public bool Flag { get => GetFlag(0); set => SetFlag(0, value); }
        public byte MultiByte { get => GetBitRange(1, 3); set => SetBitRange(1, 3, value); }
        public bool FourthBit { get => GetFlag(4); set => SetFlag(4, value); } //Yes it's the 5th bit but shush

        public TestRegister(byte initialValues) : base(initialValues)
        {
        }

        public TestRegister() : base(0x00)
        {
        }

    }
}