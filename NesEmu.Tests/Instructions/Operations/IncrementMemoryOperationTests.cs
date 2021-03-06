using System.Reflection;
using NUnit;
using NUnit.Framework;
using NesEmu.Core;
using NesEmu.Devices.CPU.Instructions.Operations;
using Moq;

namespace NesEmu.Tests.Instructions.Operations
{
    [TestFixture]
    public class IncrementMemoryOperationTests
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }        

        [Test]
        public void IncrementMemory_Should_IncrementAtMemoryLocation()
        {
            ushort address = 0xFF00;
            byte data = 0x02;
            
            var registers = new CPURegisters();

            _bus.Setup(x => x.Read(address)).Returns(data);
            _bus.Setup(x => x.Write(address, It.IsAny<byte>())).Verifiable();

            new IncrementMemoryOperation().Operate(address, registers, _bus.Object);

            _bus.Verify(x => x.Write(address, (byte)(data + 1)), Times.Once);
            Assert.False(registers.StatusRegister.Negative);
            Assert.False(registers.StatusRegister.Zero);
        }

        [Test]
        public void IncrementMemory_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
        {
            ushort address = 0xFF00;
            byte data = 0xFE;
            
            var registers = new CPURegisters();

            _bus.Setup(x => x.Read(address)).Returns(data);
            _bus.Setup(x => x.Write(address, It.IsAny<byte>())).Verifiable();

            new IncrementMemoryOperation().Operate(address, registers, _bus.Object);

            Assert.True(registers.StatusRegister.Negative);
        }

        [Test]
        public void IncrementMemory_Should_SetZeroFlag_When_OperationResultsInZeroResult()
        {
            ushort address = 0xFF00;
            byte data = 0xFF;
           
            var registers = new CPURegisters();

            _bus.Setup(x => x.Read(address)).Returns(data);
            _bus.Setup(x => x.Write(address, It.IsAny<byte>())).Verifiable();

            new IncrementMemoryOperation().Operate(address, registers, _bus.Object);

            Assert.True(registers.StatusRegister.Zero);
        }
    }
}