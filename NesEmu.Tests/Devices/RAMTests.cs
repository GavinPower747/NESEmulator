using System.Reflection;
using NesEmu.Core;
using NesEmu.Devices;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace NesEmu.Tests.Devices;

public class RAMTests
{
    private readonly IBus _bus;

    public RAMTests()
    {
        _bus = Substitute.For<IBus>();
    }

    [Fact]
    public void Read_Returns_Correct_ValueBelow2KB()
    {
        ushort address = 0x0010;
        byte memoryVal = 0xFF;
        var ramData = new byte[2048];

        ramData[address] = memoryVal;

        var ram = new Ram();
        SetRamData(ram, ramData);

        var data = ram.ReadCpu(address);

        data.Should().Be(memoryVal);
    }

    [Fact]
    public void Read_Returns_Correct_ValueAbove2KB()
    {
        ushort address = 3000;
        byte memoryVal = 0xFF;
        var ramData = new byte[2048];

        ramData[address - 2048] = memoryVal;

        var ram = new Ram();
        SetRamData(ram, ramData);

        var data = ram.ReadCpu(address);

        data.Should().Be(memoryVal);
    }

    [Fact]
    public void Write_Sets_CorrectLocation_Below2KB()
    {
        ushort address = 0x0010;
        byte memoryVal = 0xFF;

        var ram = new Ram();
        ram.WriteCpu(address, memoryVal);

        var data = ram.ReadCpu(address);

        data.Should().Be(memoryVal);
    }

    [Fact]
    public void Write_Sets_CorrectLocation_Above2KB()
    {
        ushort address = 3000;
        byte memoryVal = 0xFF;

        var ram = new Ram();
        ram.WriteCpu(address, memoryVal);

        var data = ram.ReadCpu((ushort)(address - 2048));

        data.Should().Be(memoryVal);
    }

    private void SetRamData(Ram ram, byte[] data)
    {
        var type = ram.GetType();
        var field = type.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);

        field.SetValue(ram, data);
    }
}