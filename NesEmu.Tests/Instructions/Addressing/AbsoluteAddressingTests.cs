using NesEmu.Core;
using NesEmu.Instructions.Addressing;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Addressing
{
    [TestFixture]
    public class AbsoluteAddressingTests
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }

        [Test]
        public void AbsoluteAddressing_Returns_CorrectAddress()
        {
            var strat = new AbsoluteAddressing();
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x00;

            byte expectedAddress = 0x0011;

            _bus.Setup(x => x.Read((byte)(registers.ProgramCounter))).Returns(expectedAddress);

            var addressInfo = strat.GetOperationAddress(registers, _bus.Object);

            Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
            Assert.That(addressInfo.extraCycles, Is.EqualTo(0));
        }
    }
}