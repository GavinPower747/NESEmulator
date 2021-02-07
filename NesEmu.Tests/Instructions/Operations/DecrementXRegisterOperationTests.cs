using System;
using NesEmu.Core;
using NesEmu.Instructions.Operations;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Operations
{
    [TestFixture]
    public class DecrementXRegisterOperationTests
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }        

        [Test]
        public void DecrementXRegister_Should_DecrementRegister()
        {
            var registers = new CPURegisters();
            registers.X = (byte)0x02;

            _bus.Setup(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>())).Verifiable();

            new DecrementXRegisterOperation().Operate(0x00, registers, _bus.Object);

            _bus.Verify(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
            Assert.That(registers.X, Is.EqualTo(0x02 - 1));
            Assert.False(registers.StatusRegister.HasFlag(StatusRegister.Negative));
            Assert.False(registers.StatusRegister.HasFlag(StatusRegister.Zero));
        }

        [Test]
        public void DecrementXRegister_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
        {
            var registers = new CPURegisters();
            registers.X = (byte)0x00;

            new DecrementXRegisterOperation().Operate(0x00, registers, _bus.Object);

            Assert.True(registers.StatusRegister.HasFlag(StatusRegister.Negative));
        }

        [Test]
        public void DecrementXRegister_Should_SetZeroFlag_When_OperationResultsInZeroResult()
        {
            var registers = new CPURegisters();
            registers.X = (byte)0x01;

            new DecrementXRegisterOperation().Operate(0x00, registers, _bus.Object);

            Assert.True(registers.StatusRegister.HasFlag(StatusRegister.Zero));
        }
    }
}