using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NUnit.Framework;

namespace NesEmu.Tests.Instructions.Addressing;

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
    public void ImpliedAddressing_ShouldNot_ModifyStatus()
    {
        var registers = new CPURegisters();
        var status = new StatusRegister(0x00);

        status.Zero = true;

        registers.ProgramCounter = 0x00;
        registers.StatusRegister = status;

        var sut = new ImpliedAddressing();
        var result = sut.GetOperationAddress(registers, _cpuBus);

        Assert.That(registers.StatusRegister, Is.EqualTo(status));
    }

    [Test]
    public void ImpliedAddressing_ShouldNot_Modify_ProgramCounter()
    {
        var registers = new CPURegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;

        var sut = new ImpliedAddressing();
        var result = sut.GetOperationAddress(registers, _cpuBus);

        Assert.That(registers.ProgramCounter, Is.EqualTo(initialProgramCounter));
    }
}