using NesEmu.Core;
using NesEmu.Instructions.Operations;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Operations
{
    [TestFixture]
    public class DecrementYRegisterTests
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }        

        [Test]
        public void DecrementYRegister_Should_DecrementRegister()
        {
            var registers = new CPURegisters();
            registers.Y = (byte)0x02;

            _bus.Setup(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>())).Verifiable();

            new DecrementYRegisterOperation().Operate(0x00, registers, _bus.Object);

            _bus.Verify(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never);
            Assert.That(registers.Y, Is.EqualTo(0x02 - 1));
            Assert.False(registers.StatusRegister.HasFlag(StatusRegister.Negative));
            Assert.False(registers.StatusRegister.HasFlag(StatusRegister.Zero));
        }

        [Test]
        public void DecrementYRegister_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
        {
            var registers = new CPURegisters();
            registers.Y = (byte)0x00;

            new DecrementYRegisterOperation().Operate(0x00, registers, _bus.Object);

            Assert.True(registers.StatusRegister.HasFlag(StatusRegister.Negative));
        }

        [Test]
        public void DecrementYRegister_Should_SetZeroFlag_When_OperationResultsInZeroResult()
        {
            var registers = new CPURegisters();
            registers.Y = (byte)0x01;

            new DecrementYRegisterOperation().Operate(0x00, registers, _bus.Object);

            Assert.True(registers.StatusRegister.HasFlag(StatusRegister.Zero));
        }
    }
}