using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
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
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x00;
            registers.X = 0x01;

            byte arg = 0x01;
            byte lowData = 0x11;
            byte hiData = 0x22;
            ushort expectedLowAddress = (ushort)((arg + 0x01) & 0x00FF);
            ushort expectedHiAddress = (ushort)((arg + 0x01 + 1) & 0x00FF);

            _cpuBus.Setup(x => x.ReadByte((ushort)(registers.ProgramCounter))).Returns(arg);
            _cpuBus.Setup(x => x.ReadByte(expectedLowAddress)).Returns(lowData);
            _cpuBus.Setup(x => x.ReadByte(expectedHiAddress)).Returns(hiData);

            var expectedAddress = (ushort)((hiData << 8) | lowData);

            var addressInfo = new IndirectXAddressing().GetOperationAddress(registers, _cpuBus.Object);

            Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
            Assert.That(addressInfo.extraCycles, Is.EqualTo(0));
        }
    }
}