using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Operations
{
    public class DecrementYRegisterTests
    {
        private readonly IBus _bus;

        public DecrementYRegisterTests()
        {
            _bus = Substitute.For<IBus>();
        }

        [Fact]
        public void DecrementYRegister_Should_DecrementRegister()
        {
            var registers = new CPURegisters
            {
                Y = 0x02
            };

            new DecrementYRegisterOperation().Operate(0x00, registers, _bus);

            _bus.DidNotReceive().Write(Arg.Any<ushort>(), Arg.Any<byte>());
            registers.Y.Should().Be(0x02 - 1);
            registers.StatusRegister.Negative.Should().BeFalse();
            registers.StatusRegister.Zero.Should().BeFalse();
        }

        [Fact]
        public void DecrementYRegister_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
        {
            var registers = new CPURegisters
            {
                Y = 0x00
            };

            new DecrementYRegisterOperation().Operate(0x00, registers, _bus);

            registers.StatusRegister.Negative.Should().BeTrue();
        }

        [Fact]
        public void DecrementYRegister_Should_SetZeroFlag_When_OperationResultsInZeroResult()
        {
            var registers = new CPURegisters
            {
                Y = 0x01
            };

            new DecrementYRegisterOperation().Operate(0x00, registers, _bus);

            registers.StatusRegister.Zero.Should().BeTrue();
        }
    }
}