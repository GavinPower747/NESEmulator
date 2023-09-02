using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace NesEmu.Tests.Instructions.Addressing;

public class ZeroPageAddressingTests
{
    private readonly IBus _bus;

    public ZeroPageAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void ZeroPageAddressing_Returns_CorrectAddress()
    {
        var registers = new CpuRegisters();

        registers.ProgramCounter = 0x0000;

        _bus.ReadByte(0x0000).Returns((byte)0x0011);

        ushort expectedAddress = 0x0011 & 0x00FF;

        var (address, cycles) = new ZeroPageXOffsetAddressing().GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        cycles.Should().Be(0);
    }
}