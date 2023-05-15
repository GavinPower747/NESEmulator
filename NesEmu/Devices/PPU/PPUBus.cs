using System;
using System.Collections.Generic;
using System.Linq;
using NesEmu.Core;

namespace NesEmu.Devices.PPU;

public class PPUBus : IBus
{
    private List<IPPUAddressableDevice> _devices;

    public void ConnectDevice(IAddressableDevice device)
    {
        if (device is IPPUAddressableDevice ppuDevice)
        {
            //TODO: check to make sure not overlapping ... blah.. blah
            _devices.Add(ppuDevice);
        }

        throw new ArgumentException("Trying to connect a non-PPU addressable device to the PPU Bus");
    }

    public byte ReadByte(ushort address)
    {
        var device = _devices.FirstOrDefault(x => x.PPURange.ContainsAddress(address));

        if (device is not null)
        {
            return device.ReadPPU(address);
        }

        return 0;
    }

    public ushort ReadWord(ushort address)
    {
        throw new System.NotImplementedException();
    }

    public void Write(ushort address, byte data)
    {
        var device = _devices.FirstOrDefault(x => x.PPURange.ContainsAddress(address));

        if (device is not null)
        {
            device.WritePPU(address, data);
        }
    }
}