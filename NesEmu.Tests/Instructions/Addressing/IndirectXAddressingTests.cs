using NesEmu.Core;
using NesEmu.Instructions.Addressing;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Addressing
{
    [TestFixture]
    public class IndirectXAddressingTests
    {
        private Mock<IBus> _cpuBus;

        [SetUp]
        public void Setup()
        {
            _cpuBus = new Mock<IBus>();
        }

        [Test]
        public void IndirectXAddressing_Returns_CorrectAddress()
        {
            var sut = new IndirectXAddressing();
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x00;
            registers.X = 0x01;

            ushort expectedLowAddress = 0x01;
            ushort expectedHiAddress = (ushort)(0x01 + 1);
            byte lowData = 0x11;
            byte hiData = 0xFF;

            _cpuBus.Setup(x => x.Read(expectedLowAddress)).Returns(lowData).Verifiable();
            _cpuBus.Setup(x => x.Read(expectedHiAddress)).Returns(hiData).Verifiable();

            var expectedAddress = (ushort)((hiData << 8) | lowData);

            var addressInfo = sut.GetOperationAddress(registers, _cpuBus.Object);

            Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
            Assert.That(addressInfo.extraCycles, Is.EqualTo(0));


        }
    }
}