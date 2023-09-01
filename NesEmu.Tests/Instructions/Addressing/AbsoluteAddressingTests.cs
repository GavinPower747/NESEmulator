using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace NesEmu.Tests.Instructions.Addressing;

public class AbsoluteAddressingTests
{
    private readonly IBus _bus;

    public AbsoluteAddressingTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void AbsoluteAddressing_Returns_CorrectAddress()
    {
        var strat = new AbsoluteAddressing();
        var registers = new CPURegisters
        {
            ProgramCounter = 0x00
        };

        byte expectedAddress = 0x0011;

        _bus.ReadWord((byte)registers.ProgramCounter).Returns(expectedAddress);

        var (address, cycles) = strat.GetOperationAddress(registers, _bus);

        address.Should().Be(expectedAddress);
        cycles.Should().Be(0);
    }

    [Fact]
    public void AbsoluteAddressing_Increments_ProgramCounter_ByTwo()
    {
        var strat = new AbsoluteAddressing();
        var registers = new CPURegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;

        ushort expectedAddress = 0x0011;

        _bus.ReadWord((byte)registers.ProgramCounter).Returns(expectedAddress);

        var addressInfo = strat.GetOperationAddress(registers, _bus);

        registers.ProgramCounter.Should().Be((ushort)(initialProgramCounter + 2));
    }
}