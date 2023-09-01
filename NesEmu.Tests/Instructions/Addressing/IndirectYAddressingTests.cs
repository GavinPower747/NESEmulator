using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class IndirectYAddressingTests
{
    private readonly IBus _bus;

    public IndirectYAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void IndirectYAddressing_Returns_CorrectAddress()
    {
        var sut = new IndirectYAddressing();
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            Y = 0x01
        };

        byte arg = 0x01;
        byte lowData = 0x11;
        byte hiData = 0x22;
        ushort expectedLowAddress = arg;
        ushort expectedHiAddress = (ushort)((arg + 1) & 0x00FF);

        _bus.ReadByte(registers.ProgramCounter).Returns(arg);
        _bus.ReadByte(expectedLowAddress).Returns(lowData);
        _bus.ReadByte(expectedHiAddress).Returns(hiData);

        var expectedAddress = (ushort)(((hiData << 8) | lowData) + registers.Y);

        var (address, extraCycles) = sut.GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        extraCycles.Should().Be(0);
    }

    [Fact]
    public void IndirectYAddressing_Returns_ExtraCycle_When_OffsetGoesToNextPage()
    {
        var sut = new IndirectYAddressing();
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            Y = 0x01
        };

        byte arg = 0x01;
        byte lowData = 0xFF;
        byte hiData = 0xFF;
        ushort expectedLowAddress = arg;
        ushort expectedHiAddress = (ushort)((arg + 1) & 0x00FF);

        _bus.ReadByte(registers.ProgramCounter).Returns(arg);
        _bus.ReadByte(expectedLowAddress).Returns(lowData);
        _bus.ReadByte(expectedHiAddress).Returns(hiData);

        var expectedAddress = (ushort)(((hiData << 8) | lowData) + registers.Y);

        var (address, extraCycles) = sut.GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        extraCycles.Should().Be(1);
    }
}