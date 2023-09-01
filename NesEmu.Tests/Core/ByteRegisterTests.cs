using NesEmu.Core;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Core;

public class ByteRegisterTests
{
    [Fact]
    public void Can_Set_Flag()
    {
        TestRegister testRegister = new()
        {
            FourthBit = true
        };

        testRegister.FourthBit.Should().BeTrue();
    }

    [Fact]
    public void Can_Get_Flag()
    {
        byte testByte = 0b000010000;
        TestRegister testRegister = new(testByte);

        testRegister.FourthBit.Should().BeTrue();
    }

    [Fact]
    public void Can_Set_BitRange()
    {
        TestRegister testRegister = new(0b11111111);

        testRegister.MultiByte = 0b101;

        testRegister.MultiByte.Should().Be(0b101);
    }

    [Fact]
    public void Set_DoesntChange_OtherBits()
    {
        TestRegister register = new(0b11111111);

        register.MultiByte = 0b000;
        byte backingByte = register;

        backingByte.Should().Be(0b11110001);
    }

    [Fact]
    public void Set_DoesntChange_OtherBits_When_BitIsOversized()
    {
        TestRegister register = new(0b11111111);

        register.MultiByte = 0b00001011;
        byte backingByte = register;

        backingByte.Should().Be(0b11110111);
    }

    [Fact]
    public void Can_Get_BitRange()
    {
        byte testByte = 0b000001010;
        TestRegister testRegister = new(testByte);

        testRegister.MultiByte.Should().Be(0b101);
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