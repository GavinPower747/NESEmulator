using System;
using NesEmu.Core;
using NesEmu.Extensions;
using NesEmu.Instructions.Operations;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Operations
{
    [TestFixture]
    public class ClearCarryFlagOperationTests
    {
        [Test]
        public void ClearCarryFlagOperation_DoesNot_WriteToBus()
        {
            var bus = new Mock<IBus>();
            var registers = new CPURegisters();

            bus.Setup(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>())).Verifiable();

            var extraCycles = new ClearCarryFlagOperation().Operate(0x00, registers, bus.Object);

            bus.Verify(x => x.Write(It.IsAny<ushort>(), It.IsAny<byte>()), Times.Never());
        }

        [Test]
        public void ClearCarryFlagOperation_ClearsCarryFlag()
        {
            var bus = new Mock<IBus>();
            var registers = new CPURegisters();

            registers.StatusRegister.SetFlag(StatusRegister.Carry, true);

            var extraCycles = new ClearCarryFlagOperation().Operate(0x00, registers, bus.Object);

            Assert.That(registers.StatusRegister.HasFlag(StatusRegister.Carry), Is.False);
        }
    }
}