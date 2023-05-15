using NUnit.Framework;
using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;


namespace NesEmu.Tests.Instructions.Addressing;


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
    public void ImmediateAddressing_Should_Return_ProgramCounter()
    {
        //The program counter should have already been incremented before the address is retrieved
        //so just return the program counter rather than program counter + 1
        var registers = new CPURegisters();

        registers.ProgramCounter = 0x00;

        var sut = new ImmediateAddressing();
        var result = sut.GetOperationAddress(registers, _cpuBus);

        Assert.That(result.address, Is.EqualTo(0x00));
        Assert.That(result.extraCycles, Is.EqualTo(0));
    }

    [Test]
    public void ImmediateAddressing_ShouldNot_ModifyStatus()
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

    [Test]
    public void ImmediateAddressing_Should_Increment_ProgramCounter_ByOne()
    {
        var registers = new CPURegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;

        var sut = new ImmediateAddressing();
        var result = sut.GetOperationAddress(registers, _cpuBus);

        Assert.That(registers.ProgramCounter, Is.EqualTo(initialProgramCounter + 1));
    }
}