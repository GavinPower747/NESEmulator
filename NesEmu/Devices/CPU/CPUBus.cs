using System;
using System.Collections.Generic;
using System.Linq;
using NesEmu.Core;

namespace NesEmu.Devices.CPU;

internal class CpuBus : IBus
{
    private readonly Cpu _cpu;
    private readonly List<ICPUAddressableDevice> _connectedDevices;

    internal CpuBus(Cpu cpu)
    {
        _cpu = cpu;
        _connectedDevices = new List<ICPUAddressableDevice>();
        _cpu.ConnectBus(this);
    }

    public byte ReadByte(ushort address)
    {
        var device = _connectedDevices.FirstOrDefault(x => x.CpuRange.ContainsAddress(address));

        if (device is null)
            return 0;

        return device.ReadCpu(address);
    }

    public ushort ReadWord(ushort address)
    {
        var device = _connectedDevices.FirstOrDefault(x => x.CpuRange.ContainsAddress(address));

        if (device is null)
            return 0;

        var lo = (ushort)device.ReadCpu(address);
        var hi = (ushort)device.ReadCpu((ushort)(address + 1));

        return (ushort)(hi << 8 | lo);;
    }

    public void Write(ushort address, byte data)
    {
        var device = _connectedDevices.FirstOrDefault(x => x.CpuRange.ContainsAddress(address));

        device?.WriteCpu(address, data);
    }

    public void ConnectDevice(IAddressableDevice device)
    {
        if (device is not ICPUAddressableDevice cpuDevice)
            throw new ArgumentException("Attempting to add non-cpu device to CPU Bus");

        //TODO: Validate devices to make sure ranges aren't overlapping
        _connectedDevices.Add(cpuDevice);
    }
}