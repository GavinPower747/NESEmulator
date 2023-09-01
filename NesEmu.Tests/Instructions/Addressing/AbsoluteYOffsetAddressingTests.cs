using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class AbsoluteYOffsetAddressingTests
{
    private readonly IBus _bus;

    public AbsoluteYOffsetAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void AbsoluteAddressingYOffset_Returns_CorrectAddress()
    {
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            Y = 0x01
        };

        byte arg = 0x0011;
        byte expectedAddress = (byte)(arg + 0x01);

        _bus.ReadWord(registers.ProgramCounter).Returns(arg);

        var (address, extraCycles) = new AbsoluteYOffsetAddressing().GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        extraCycles.Should().Be(0);
    }

    [Fact]
    public void AbsoluteAddressingYOffset_Returns_ExtraCycle_When_OffsetGoesToNextPage()
    {
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00,
            Y = 0xFF
        };

        byte arg = 0x01;
        ushort expectedAddress = (ushort)(arg + 0xFF);

        _bus.ReadWord(registers.ProgramCounter).Returns(arg);

        var (address, extraCycles) = new AbsoluteYOffsetAddressing().GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        extraCycles.Should().Be(1);
    }

    [Fact]
    public void AbsoluteAddressingYOffset_Increments_ProgramCounter_ByTwo()
    {
        var registers = new CPURegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;
        registers.Y = 0xFF;

        byte arg = 0x01;

        _bus.ReadWord(registers.ProgramCounter).Returns(arg);

        _ = new AbsoluteYOffsetAddressing().GetOperationAddress(registers, _bus);

        registers.ProgramCounter.Should().Be((ushort)(initialProgramCounter + 2));
    }
}