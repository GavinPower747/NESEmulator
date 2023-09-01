using FluentAssertions;
using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using NSubstitute;
using Xunit;

namespace NesEmu.Tests.Instructions.Operations
{
    public class IncrementXRegisterOperationTests
    {
        private readonly IBus _bus;

        public IncrementXRegisterOperationTests()
        {
            _bus = Substitute.For<IBus>();
        }

        [Fact]
        public void IncrementXRegister_Should_IncrementRegister()
        {
            var registers = new CPURegisters
            {
                X = 0x02
            };

            _bus.Write(Arg.Any<ushort>(), Arg.Any<byte>());

            new IncrementXRegisterOperation().Operate(0x00, registers, _bus);

            _bus.DidNotReceive().Write(Arg.Any<ushort>(), Arg.Any<byte>());
            registers.X.Should().Be(0x03);
            registers.StatusRegister.Negative.Should().BeFalse();
            registers.StatusRegister.Zero.Should().BeFalse();
        }

        [Fact]
        public void IncrementXRegister_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
        {
            var registers = new CPURegisters
            {
                X = 0xFE
            };

            new IncrementXRegisterOperation().Operate(0x00, registers, _bus);

            registers.StatusRegister.Negative.Should().BeTrue();
        }

        [Fact]
        public void IncrementXRegister_Should_SetZeroFlag_When_OperationResultsInZeroResult()
        {
            var registers = new CPURegisters
            {
                X = 0xFF
            };

            new IncrementXRegisterOperation().Operate(0x00, registers, _bus);

            registers.StatusRegister.Zero.Should().BeTrue();
        }
    }
}