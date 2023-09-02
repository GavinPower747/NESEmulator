using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Operations;

public class ClearCarryFlagOperationTests
{
    private readonly IBus _bus;

    public ClearCarryFlagOperationTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void ClearCarryFlagOperation_DoesNot_WriteToBus()
    {
        var registers = new CpuRegisters();

        new ClearCarryFlagOperation().Operate(0x00, registers, _bus);

        _bus.DidNotReceive().Write(Arg.Any<ushort>(), Arg.Any<byte>());
    }

    [Fact]
    public void ClearCarryFlagOperation_ClearsCarryFlag()
    {
        var registers = new CpuRegisters
        {
            StatusRegister = new StatusRegister(0x00)
            {
                Carry = true
            }
        };

        new ClearCarryFlagOperation().Operate(0x00, registers, _bus);

        registers.StatusRegister.Carry.Should().BeFalse();
    }
}