using NesEmu.Devices.CPU;
using NesEmu.Devices.CPU.Instructions.Addressing;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Instructions.Addressing;

public class ImpliedAddressingTests
{
    private readonly CpuBus _cpuBus;

    public ImpliedAddressingTests()
    {
        var cpu = new Cpu();
        _cpuBus = new CpuBus(cpu);
    }

    [Fact]
    public void ImpliedAddressing_Returns_ProgramCounter()
    {
        var registers = new CpuRegisters
        {
            ProgramCounter = 0x00
        };

        var sut = new ImpliedAddressing();
        var (address, extraCycles) = sut.GetOperationAddress(registers, _cpuBus);

        address.Should().Be(registers.ProgramCounter);
        extraCycles.Should().Be(0);
    }

    [Fact]
    public void ImpliedAddressing_ShouldNot_ModifyStatus()
    {
        var registers = new CpuRegisters();
        var status = new StatusRegister(0x00)
        {
            Zero = true
        };

        registers.ProgramCounter = 0x00;
        registers.StatusRegister = status;

        var sut = new ImpliedAddressing();
        _ = sut.GetOperationAddress(registers, _cpuBus);

        registers.StatusRegister.Should().Be(status);
    }

    [Fact]
    public void ImpliedAddressing_ShouldNot_Modify_ProgramCounter()
    {
        var registers = new CpuRegisters();
        ushort initialProgramCounter = 0x00;

        registers.ProgramCounter = initialProgramCounter;

        var sut = new ImpliedAddressing();
        _ = sut.GetOperationAddress(registers, _cpuBus);

        registers.ProgramCounter.Should().Be(initialProgramCounter);
    }
}