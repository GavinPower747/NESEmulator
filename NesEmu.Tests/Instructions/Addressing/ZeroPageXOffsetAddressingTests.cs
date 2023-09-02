using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class ZeroPageXOffsetAddressingTests
{
    private readonly IBus _bus;

    public ZeroPageXOffsetAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void ZeroPageXOffsetAddressing_Returns_CorrectAddress()
    {
        var registers = new CpuRegisters
        {
            ProgramCounter = 0x0000,
            X = 0x0001
        };

        _bus.ReadByte(0x0000).Returns((byte)0x0011);

        ushort expectedAddress = (ushort)((0x0011 + registers.X) & 0x00FF);

        var (address, cycles) = new ZeroPageXOffsetAddressing().GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        cycles.Should().Be(0);
    }
}