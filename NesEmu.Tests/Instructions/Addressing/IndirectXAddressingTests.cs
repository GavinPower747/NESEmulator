using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class IndirectXAddressingTests
{
    private readonly IBus _bus;

    public IndirectXAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void IndirectXAddressing_Returns_CorrectAddress()
    {
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            X = 0x01
        };

        byte arg = 0x01;
        byte lowData = 0x11;
        byte hiData = 0x22;
        ushort expectedLowAddress = (ushort)((arg + 0x01) & 0x00FF);
        ushort expectedHiAddress = (ushort)((arg + 0x01 + 1) & 0x00FF);

        _bus.ReadByte(registers.ProgramCounter).Returns(arg);
        _bus.ReadByte(expectedLowAddress).Returns(lowData);
        _bus.ReadByte(expectedHiAddress).Returns(hiData);

        var expectedAddress = (ushort)((hiData << 8) | lowData);

        var (address, extraCycles) = new IndirectXAddressing().GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        extraCycles.Should().Be(0);
    }
}