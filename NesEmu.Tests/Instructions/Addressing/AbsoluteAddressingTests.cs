using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Addressing;

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

        _bus.Setup(x => x.ReadWord((byte)(registers.ProgramCounter))).Returns(expectedAddress);

        var addressInfo = strat.GetOperationAddress(registers, _bus.Object);

        Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
        Assert.That(addressInfo.extraCycles, Is.EqualTo(0));
    }

    [Test]
    public void AbsoluteAddressing_Increments_ProgramCounter_ByTwo()
    {
        var strat = new AbsoluteAddressing();
        var registers = new CPURegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;

        ushort expectedAddress = 0x0011;

        _bus.Setup(x => x.ReadWord((byte)(registers.ProgramCounter))).Returns(expectedAddress);

        var addressInfo = strat.GetOperationAddress(registers, _bus.Object);

        Assert.That(registers.ProgramCounter, Is.EqualTo(initialProgramCounter + 2));
    }
}