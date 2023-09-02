using FluentAssertions;
using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using NSubstitute;
using Xunit;

namespace NesEmu.Facts.Instructions.Operations;


public class IncrementYRegisterTests
{
    private readonly IBus _bus;

    public IncrementYRegisterTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void IncrementYRegister_Should_IncrementRegister()
    {
        var registers = new CpuRegisters
        {
            Y = 0x02
        };
    
        new IncrementYRegisterOperation().Operate(0x00, registers, _bus);
    
        _bus.DidNotReceive().Write(Arg.Any<ushort>(), Arg.Any<byte>());
        registers.Y.Should().Be(0x03);
        registers.StatusRegister.Negative.Should().BeFalse();
        registers.StatusRegister.Zero.Should().BeFalse();
    }

    [Fact]
    public void IncrementYRegister_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
    {
        var registers = new CpuRegisters
        {
            Y = 0xFE
        };

        new IncrementYRegisterOperation().Operate(0x00, registers, _bus);

        registers.StatusRegister.Negative.Should().BeTrue();
    }

    [Fact]
    public void IncrementYRegister_Should_SetZeroFlag_When_OperationResultsInZeroResult()
    {
        var registers = new CpuRegisters
        {
            Y = 0xFF
        };

        new IncrementYRegisterOperation().Operate(0x00, registers, _bus);

        registers.StatusRegister.Zero.Should().BeTrue();
    }
}