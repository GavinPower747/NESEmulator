using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Operations;

public class DecrementXRegisterOperationTests
{
    private readonly IBus _bus;

    public DecrementXRegisterOperationTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void DecrementXRegister_Should_DecrementRegister()
    {
        var registers = new CpuRegisters
        {
            X = 0x02
        };

        _ = new DecrementXRegisterOperation().Operate(0x00, registers, _bus);

        _bus.DidNotReceive().Write(Arg.Any<ushort>(), Arg.Any<byte>());
        registers.X.Should().Be(0x02 - 1);
        registers.StatusRegister.Negative.Should().BeFalse();
        registers.StatusRegister.Zero.Should().BeFalse();
    }

    [Fact]
    public void DecrementXRegister_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
    {
        var registers = new CpuRegisters
        {
            X = 0x00
        };

        _ = new DecrementXRegisterOperation().Operate(0x00, registers, _bus);

        registers.StatusRegister.Negative.Should().BeTrue();
    }

    [Fact]
    public void DecrementXRegister_Should_SetZeroFlag_When_OperationResultsInZeroResult()
    {
        var registers = new CpuRegisters
        {
            X = 0x01
        };

        _ = new DecrementXRegisterOperation().Operate(0x00, registers, _bus);

        registers.StatusRegister.Zero.Should().BeTrue();
    }
}