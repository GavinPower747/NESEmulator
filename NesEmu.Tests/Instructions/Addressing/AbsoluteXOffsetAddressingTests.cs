using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class AbsoluteXOffsetAddressingTests
{
    private readonly IBus _bus;

    public AbsoluteXOffsetAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void GetOperationAddress_Returns_CorrectAddress()
    {
        // Arrange
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            X = 0x01
        };
        byte arg = 0x0011;
        byte expectedAddress = (byte)(arg + 0x01);
        _bus.ReadWord(registers.ProgramCounter).Returns(arg);

        // Act
        var (address, cycles) = new AbsoluteXOffsetAddressing().GetOperationAddress(registers, _bus);

        // Assert
        address.Should().Be(expectedAddress);
        cycles.Should().Be(0);
    }

    [Fact]
    public void GetOperationAddress_Returns_ExtraCycle_When_OffsetGoesToNextPage()
    {
        // Arrange
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            X = 0xFF
        };
        
        byte arg = 0x01;
        ushort expectedAddress = (ushort)(arg + 0xFF);
        _bus.ReadWord(registers.ProgramCounter).Returns(arg);

        // Act
        var (address, cycles) = new AbsoluteXOffsetAddressing().GetOperationAddress(registers, _bus);

        // Assert
        address.Should().Be(expectedAddress);
        cycles.Should().Be(1);
    }

    [Fact]
    public void GetOperationAddress_Increments_ProgramCounter_ByTwo()
    {
        // Arrange
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            X = 0x01
        };
        byte arg = 0x0011;
        _bus.ReadWord(registers.ProgramCounter).Returns(arg);

        // Act
        _ = new AbsoluteXOffsetAddressing().GetOperationAddress(registers, _bus);

        // Assert
        registers.ProgramCounter.Should().Be(0x02);
    }
}