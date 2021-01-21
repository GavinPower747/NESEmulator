using NesEmu.Core;
using NesEmu.Instructions.Addressing;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Addressing
{
    [TestFixture]
    public class IndirectYAddressingTests
    {
        private Mock<IBus> _cpuBus;

        [SetUp]
        public void Setup()
        {
            _cpuBus = new Mock<IBus>();
        }

        [Test]
        public void IndirectYAddressing_Returns_CorrectAddress()
        {
            var sut = new IndirectYAddressing();
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x00;
            registers.Y = 0x01;

            byte arg = 0x01;
            byte lowData = 0x11;
            byte hiData = 0x22;
            ushort expectedLowAddress = (ushort)(arg);
            ushort expectedHiAddress = (ushort)((arg + 1) & 0x00FF);

            _cpuBus.Setup(x => x.Read((ushort)(registers.ProgramCounter))).Returns(arg);
            _cpuBus.Setup(x => x.Read(expectedLowAddress)).Returns(lowData).Verifiable();
            _cpuBus.Setup(x => x.Read(expectedHiAddress)).Returns(hiData).Verifiable();

            var expectedAddress = (ushort)(((hiData << 8) | lowData) + registers.Y);

            var addressInfo = sut.GetOperationAddress(registers, _cpuBus.Object);

            Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
            Assert.That(addressInfo.extraCycles, Is.EqualTo(0));


        }

        [Test]
        public void IndirectYAddressing_Returns_ExtraCycle_When_OffsetGoesToNextPage()
        {
            var sut = new IndirectYAddressing();
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x00;
            registers.Y = 0x01;

            byte arg = 0x01;
            byte lowData = 0xFF;
            byte hiData = 0xFF;
            ushort expectedLowAddress = (ushort)(arg);
            ushort expectedHiAddress = (ushort)((arg + 1) & 0x00FF);

            _cpuBus.Setup(x => x.Read((ushort)(registers.ProgramCounter))).Returns(arg);
            _cpuBus.Setup(x => x.Read(expectedLowAddress)).Returns(lowData).Verifiable();
            _cpuBus.Setup(x => x.Read(expectedHiAddress)).Returns(hiData).Verifiable();

            var expectedAddress = (ushort)(((hiData << 8) | lowData) + registers.Y);

            var addressInfo = sut.GetOperationAddress(registers, _cpuBus.Object);

            Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
            Assert.That(addressInfo.extraCycles, Is.EqualTo(1));
        }
    }
}