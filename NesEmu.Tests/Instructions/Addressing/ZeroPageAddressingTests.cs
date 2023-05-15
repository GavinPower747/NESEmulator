using NesEmu.Core;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NUnit;
using NUnit.Framework;
using Moq;

namespace NesEmu.Tests.Instructions.Addressing;

[TestFixture]
public class ZeroPageAddressingTests
{
    private Mock<IBus> _bus;

    [SetUp]
    public void Setup()
    {
        _bus = new Mock<IBus>();
    }

    [Test]
    public void ZeroPageAddressing_Returns_CorrectAddress()
    {
        var registers = new CPURegisters();

        registers.ProgramCounter = 0x0000;

        _bus.Setup(x => x.ReadByte(0x0000)).Returns(0x0011);

        ushort expectedAddress = (ushort)(0x0011 & 0x00FF);

        var addressInfo = new ZeroPageXOffsetAddressing().GetOperationAddress(registers, _bus.Object);

        Assert.That(addressInfo.address, Is.EqualTo(expectedAddress));
        Assert.That(addressInfo.extraCycles, Is.EqualTo(0));
    }
}