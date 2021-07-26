using NUnit;
using NUnit.Framework;
using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Extensions;


namespace NesEmu.Tests.Instructions.Addressing
{

    [TestFixture]
    public class ImmediateAddressingTests
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
        public void ImmediateAddressing_Should_Return_FollowingMemoryAddress()
        {
            var registers = new CPURegisters();

            registers.ProgramCounter = 0x00;

            var sut = new ImmediateAddressing();
            var result = sut.GetOperationAddress(registers, _cpuBus);

            Assert.That(result.address, Is.EqualTo(registers.ProgramCounter + 1));
            Assert.That(result.extraCycles, Is.EqualTo(0));
        }

        [Test]
        public void ImmediateAddressing_ShoutNot_ModifyStatus()
        {
            var registers = new CPURegisters();
            var status = new StatusRegister(0x00);
            
            status.Zero = true;

            registers.ProgramCounter = 0x00;
            registers.StatusRegister = status;

            var sut = new ImmediateAddressing();
            var result = sut.GetOperationAddress(registers, _cpuBus);

            Assert.That(registers.StatusRegister, Is.EqualTo(status));
        }
    }
}