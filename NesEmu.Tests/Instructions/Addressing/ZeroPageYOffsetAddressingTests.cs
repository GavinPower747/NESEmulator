using NesEmu.Core;
using NesEmu.Instructions.Addressing;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Addressing
{
    [TestFixture]
    public class ZeroPageYOffsetAddressing
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }

        [Test]
        public void ZeroPageYOffsetAddressing_Returns_CorrectValue()
        {
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x0000;
            registers.Y = 0x0001;

            _bus.Setup(x => x.Read(0x0000)).Returns(0x0011);

            ushort expectedAddress = (ushort)((0x0011 + registers.X) & 0x00FF);

            var addressInfo = new ZeroPageXOffsetAddressing().GetOperationAddress(registers, _bus.Object);

            Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
            Assert.That(addressInfo.extraCycles, Is.EqualTo(0));
        }
    }
}