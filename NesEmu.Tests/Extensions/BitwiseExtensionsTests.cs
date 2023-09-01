using NesEmu.Extensions;
using FluentAssertions;
using Xunit;

namespace NesEmu.Tests.Extensions;

public class BitwiseExtensionsTests
{
    [Fact]
    public void IsNegative_Should_ReturnTrue_When_ValueIsNegative()
    {
        byte value = 0b1000_0000;

        value.IsNegative().Should().BeTrue();
    }

    [Fact]
    public void IsNegative_Should_ReturnFalse_When_ValueIsPositive()
    {
        byte value = 0b0000_0000;

        value.IsNegative().Should().BeFalse();
    }

    [Fact]
    public void IsNegative_Should_ReturnFalse_When_ValueIsZero()
    {
        byte value = 0b0000_0000;

        value.IsNegative().Should().BeFalse();
    }

    [Fact]
    public void IsNegative_Should_ReturnTrue_When_ValueIsSetAndNegativeBitIsSet()
    {
        byte value = 0b1000_0001;

        value.IsNegative().Should().BeTrue();
    }
}
