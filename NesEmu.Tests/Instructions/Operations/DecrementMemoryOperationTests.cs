using System.Reflection;
using NUnit;
using NUnit.Framework;
using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Operations;
using Moq;

namespace NesEmu.Tests.Instructions.Operations
{
    [TestFixture]
    public class DecrementMemoryOperationTests
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }        

        [Test]
        public void DecrementMemory_Should_DecrementAtMemoryLocation()
        {
            ushort address = 0xFF00;
            byte data = 0x02;
            
            var registers = new CPURegisters();

            _bus.Setup(x => x.ReadByte(address)).Returns(data);
            _bus.Setup(x => x.Write(address, It.IsAny<byte>())).Verifiable();

            new DecrementMemoryOperation().Operate(address, registers, _bus.Object);

            _bus.Verify(x => x.Write(address, (byte)(data - 1)), Times.Once);
            Assert.False(registers.StatusRegister.Negative);
            Assert.False(registers.StatusRegister.Zero);
        }

        [Test]
        public void DecrementMemory_Should_SetNegativeFlag_When_OperationResultsInNegativeResult()
        {
            ushort address = 0xFF00;
            byte data = 0x00;
            
            var registers = new CPURegisters();

            _bus.Setup(x => x.ReadByte(address)).Returns(data);
            _bus.Setup(x => x.Write(address, It.IsAny<byte>())).Verifiable();

            new DecrementMemoryOperation().Operate(address, registers, _bus.Object);

            Assert.True(registers.StatusRegister.Negative);
        }

        [Test]
        public void DecrementMemory_Should_SetZeroFlag_When_OperationResultsInZeroResult()
        {
            ushort address = 0xFF00;
            byte data = 0x01;
           
            var registers = new CPURegisters();

            _bus.Setup(x => x.ReadByte(address)).Returns(data);
            _bus.Setup(x => x.Write(address, It.IsAny<byte>())).Verifiable();

            new DecrementMemoryOperation().Operate(address, registers, _bus.Object);

            Assert.True(registers.StatusRegister.Zero);
        }
    }
}