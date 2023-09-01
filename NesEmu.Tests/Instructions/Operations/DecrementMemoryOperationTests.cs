using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Operations;

public class DecrementMemoryOperationTests
{
    private readonly IBus _bus;

    public DecrementMemoryOperationTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void DecrementMemory_Should_DecrementAtMemoryLocation()
    {
        ushort address = 0xFF00;
        byte data = 0x02;

        var registers = new CPURegisters();

        _bus.ReadByte(address).Returns(data);
        _bus.When(x => x.Write(address, Arg.Any<byte>()));

        new DecrementMemoryOperation().Operate(address, registers, _bus);

        _bus.Received(1).Write(address, (byte)(data - 1));
        registers.StatusRegister.Negative.Should().BeFalse();
        registers.StatusRegister.Zero.Should().BeFalse();
    }

    [Fact]
    public void DecrementMemory_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
    {
        ushort address = 0xFF00;
        byte data = 0x00;

        var registers = new CPURegisters();

        _bus.ReadByte(address).Returns(data);
        _bus.When(x => x.Write(address, Arg.Any<byte>())).Do(x => data = x.Arg<byte>());

        new DecrementMemoryOperation().Operate(address, registers, _bus);

        registers.StatusRegister.Negative.Should().BeTrue();
    }

    [Fact]
    public void DecrementMemory_Should_SetZeroFlag_When_OperationResultsInZeroResult()
    {
        ushort address = 0xFF00;
        byte data = 0x01;

        var registers = new CPURegisters();

        _bus.ReadByte(address).Returns(data);
        _bus.When(x => x.Write(address, Arg.Any<byte>())).Do(x => data = x.Arg<byte>());

        new DecrementMemoryOperation().Operate(address, registers, _bus);

        registers.StatusRegister.Zero.Should().BeTrue();
    }
}