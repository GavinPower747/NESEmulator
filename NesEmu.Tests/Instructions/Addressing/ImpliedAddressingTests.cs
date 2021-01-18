using NesEmu.Core;
using NesEmu.Extensions;
using NesEmu.Instructions.Addressing;
using NUnit;
using NUnit.Framework;

namespace NesEmu.Tests.Instructions.Addressing
{
    [TestFixture]
    public class ImpliedAddressingTests
    {
        private CPU _cpu;
        private CPUBus _cpuBus;

        [SetUp]
        public void Setup()
        {
            _cpu = new CPU();
            _cpuBus = new CPUBus(_cpu);
        }

        [Test]
        public void ImpliedAddressing_Returns_ProgramCounter()
        {
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x00;

            var sut = new ImpliedAddressing();
            var result = sut.GetOperationAddress(registers, _cpuBus);

            Assert.That(result.address, Is.EqualTo(registers.ProgramCounter));
            Assert.That(result.extraCycles, Is.EqualTo(0));
        }

        [Test]
        public void ImpliedAddressing_ShoutNot_ModifyStatus()
        {
            var registers = new CPURegisters();
            var status = new StatusRegister();
            
            status.SetFlag(StatusRegister.Zero, true);

            registers.ProgramCounter = 0x00;
            registers.StatusRegister = status;

            var sut = new ImpliedAddressing();
            var result = sut.GetOperationAddress(registers, _cpuBus);

            Assert.That(registers.StatusRegister, Is.EqualTo(status));
        }
    }
}