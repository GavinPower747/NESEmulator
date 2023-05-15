using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NUnit.Framework;
using Moq;

namespace NesEmu.Instructions.Addressing;

[TestFixture]
public class AbsoluteYOffsetAddressingTests
{
    private Mock<IBus> _bus;

    [SetUp]
    public void Setup()
    {
        _bus = new Mock<IBus>();
    }

    [Test]
    public void AbsoluteAddressingYOffset_Returns_CorrectAddress()
    {
        var registers = new CPURegisters();

        registers.ProgramCounter = 0x00;
        registers.Y = 0x01;

        byte arg = 0x0011;
        byte expectedAddress = (byte)(arg + 0x01);

        _bus.Setup(x => x.ReadWord(registers.ProgramCounter)).Returns(arg);

        var addressInfo = new AbsoluteYOffsetAddressing().GetOperationAddress(registers, _bus.Object);

        Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
        Assert.That(addressInfo.extraCycles, Is.EqualTo(0));
    }

    [Test]
    public void AbsoluteAddressingYOffset_Returns_ExtraCycle_When_OffsetGoesToNextPage()
    {
        var registers = new CPURegisters();

        registers.ProgramCounter = 0x00;
        registers.Y = 0xFF;

        byte arg = 0x01;
        ushort expectedAddress = (ushort)(arg + 0xFF);

        _bus.Setup(x => x.ReadWord(registers.ProgramCounter)).Returns(arg);

        var addressInfo = new AbsoluteYOffsetAddressing().GetOperationAddress(registers, _bus.Object);

        Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
        Assert.That(addressInfo.extraCycles, Is.EqualTo(1));
    }

    [Test]
    public void AbsoluteAddressingYOffset_Increments_ProgramCounter_ByTwo()
    {
        var registers = new CPURegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;
        registers.Y = 0xFF;

        byte arg = 0x01;
        ushort expectedAddress = (ushort)(arg + 0xFF);

        _bus.Setup(x => x.ReadWord(registers.ProgramCounter)).Returns(arg);

        var addressInfo = new AbsoluteYOffsetAddressing().GetOperationAddress(registers, _bus.Object);

        Assert.That(registers.ProgramCounter, Is.EqualTo(initialProgramCounter + 2));
    }
}