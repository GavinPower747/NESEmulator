using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class ZeroPageYOffsetAddressingTests
{
    private readonly IBus _bus;

    public ZeroPageYOffsetAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void ZeroPageYOffsetAddressing_Returns_CorrectValue()
    {
        var registers = new CpuRegisters
        {
            ProgramCounter = 0x0000,
            Y = 0x0001
        };

        _bus.ReadByte(0x0000).Returns((byte)0x0011);

        ushort expectedAddress = (ushort)((0x0011 + registers.Y) & 0x00FF);

        var (address, cycles) = new ZeroPageYOffsetAddressing().GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        cycles.Should().Be(0);
    }
}