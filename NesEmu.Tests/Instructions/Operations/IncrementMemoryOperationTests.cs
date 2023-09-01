using FluentAssertions;
using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using NSubstitute;
using Xunit;

namespace NesEmu.Tests.Instructions.Operations;

public class IncrementMemoryOperationTests
{
    private readonly IBus _bus;

    public IncrementMemoryOperationTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void IncrementMemory_Should_IncrementAtMemoryLocation()
    {
        ushort address = 0xFF00;
        byte data = 0x02;

        var registers = new CPURegisters();

        _bus.ReadByte(address).Returns(data);
        _bus.When(x => x.Write(address, Arg.Any<byte>()));

        new IncrementMemoryOperation().Operate(address, registers, _bus);

        _bus.Received(1).Write(address, (byte)(data + 1));
        registers.StatusRegister.Negative.Should().BeFalse();
        registers.StatusRegister.Zero.Should().BeFalse();
    }

    [Fact]
    public void IncrementMemory_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
    {
        ushort address = 0xFF00;
        byte data = 0xFE;

        var registers = new CPURegisters();

        _bus.ReadByte(address).Returns(data);
        _bus.When(x => x.Write(address, Arg.Any<byte>()));

        new IncrementMemoryOperation().Operate(address, registers, _bus);

        registers.StatusRegister.Negative.Should().BeTrue();
    }

    [Fact]
    public void IncrementMemory_Should_SetZeroFlag_When_OperationResultsInZeroResult()
    {
        ushort address = 0xFF00;
        byte data = 0xFF;

        var registers = new CPURegisters();

        _bus.ReadByte(address).Returns(data);
        _bus.When(x => x.Write(address, Arg.Any<byte>()));

        new IncrementMemoryOperation().Operate(address, registers, _bus);

        registers.StatusRegister.Zero.Should().BeTrue();
    }
}