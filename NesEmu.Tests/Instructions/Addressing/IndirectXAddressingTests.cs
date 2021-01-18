using NesEmu.Core;
using NesEmu.Instructions.Addressing;
using NUnit;
using NUnit.Framework;

namespace NesEmu.Tests.Instructions.Addressing
{
    [TestFixture]
    public class IndirectXAddressingTests
    {
        private CPU _cpu;
        private CPUBus _cpuBus;

        [SetUp]
        public void Setup()
        {
            _cpu = new CPU();
            _cpuBus = new CPUBus(_cpu);
        }
    }
}