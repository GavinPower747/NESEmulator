using System;
using System.Collections.Generic;
using System.Linq;
using NesEmu.Core;

namespace NesEmu.Devices.CPU;

internal class CPUBus : IBus
{
    private CPU _cpu;
    private List<ICPUAddressableDevice> _connectedDevices;

    internal CPUBus(CPU cpu)
    {
        _cpu = cpu;
        _connectedDevices = new List<ICPUAddressableDevice>();
        _cpu.ConnectBus(this);
    }

    public byte ReadByte(ushort address)
    {
        var device = _connectedDevices.FirstOrDefault(x => x.CpuRange.ContainsAddress(address));

        if (device is not null)
            return device.ReadCpu(address);

        return 0;
    }

    public ushort ReadWord(ushort address)
    {
        var device = _connectedDevices.FirstOrDefault(x => x.CpuRange.ContainsAddress(address));

        if (device is not null)
        {
            var lo = (ushort)device.ReadCpu(address);
            var hi = (ushort)device.ReadCpu((ushort)(address + 1));

            return (ushort)(hi << 8 | lo);
        }

        return 0;
    }

    public void Write(ushort address, byte data)
    {
        var device = _connectedDevices.FirstOrDefault(x => x.CpuRange.ContainsAddress(address));

        if (device is not null)
        {
            device.WriteCpu(address, data);
        }
    }

    public void ConnectDevice(IAddressableDevice device)
    {
        if (device is not ICPUAddressableDevice cpuDevice)
            throw new ArgumentException("Attempting to add non-cpu device to CPU Bus");

        //TODO: Validate devices to make sure ranges aren't overlapping
        _connectedDevices.Add(cpuDevice);
    }
}