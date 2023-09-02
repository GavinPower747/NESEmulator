using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class ImmediateAddressingTests
{
    private readonly CpuBus _cpuBus;

    public ImmediateAddressingTests()
    {
        var cpu = new Cpu();
        _cpuBus = new CpuBus(cpu);
    }

    [Fact]
    public void ImmediateAddressing_Should_Return_ProgramCounter()
    {
        //The program counter should have already been incremented before the address is retrieved
        //so just return the program counter rather than program counter + 1
        var registers = new CpuRegisters
        {
            ProgramCounter = 0x00
        };

        var sut = new ImmediateAddressing();
        var (address, extraCycles) = sut.GetOperationAddress(registers, _cpuBus);

        address.Should().Be(0x00);
        extraCycles.Should().Be(0);
    }

    [Fact]
    public void ImmediateAddressing_ShouldNot_ModifyStatus()
    {
        var registers = new CpuRegisters();
        var status = new StatusRegister(0x00)
        {
            Zero = true
        };

        registers.ProgramCounter = 0x00;
        registers.StatusRegister = status;

        var sut = new ImmediateAddressing();
        _ = sut.GetOperationAddress(registers, _cpuBus);

        registers.StatusRegister.Should().Be(status);
    }

    [Fact]
    public void ImmediateAddressing_Should_Increment_ProgramCounter_ByOne()
    {
        var registers = new CpuRegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;

        var sut = new ImmediateAddressing();
        _ = sut.GetOperationAddress(registers, _cpuBus);

        registers.ProgramCounter.Should().Be((ushort)(initialProgramCounter + 1));
    }
}