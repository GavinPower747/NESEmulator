using System;
using NesEmu.Core;
using NesEmu.Devices.CPU.Instructions.Operations;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Operations
{
    [TestFixture]
    public class IncrementXRegisterOperationTests
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }        

        [Test]
        public void IncrementXRegister_Should_IncrementRegister()
        {
            var registers = new CPURegisters();
            registers.X = (byte)0x02;

            _bus.Setup(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>())).Verifiable();

            new IncrementXRegisterOperation().Operate(0x00, registers, _bus.Object);

            _bus.Verify(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
            Assert.That(registers.X, Is.EqualTo(0x02 + 1));
            Assert.False(registers.StatusRegister.Negative);
            Assert.False(registers.StatusRegister.Zero);
        }

        [Test]
        public void IncrementXRegister_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
        {
            var registers = new CPURegisters();
            registers.X = (byte)0xFE;

            new IncrementXRegisterOperation().Operate(0x00, registers, _bus.Object);

            Assert.True(registers.StatusRegister.Negative);
        }

        [Test]
        public void IncrementXRegister_Should_SetZeroFlag_When_OperationResultsInZeroResult()
        {
            var registers = new CPURegisters();
            registers.X = (byte)(0xFF);

            new IncrementXRegisterOperation().Operate(0x00, registers, _bus.Object);

            Assert.True(registers.StatusRegister.Zero);
        }
    }
}