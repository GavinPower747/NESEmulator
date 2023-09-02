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
        if (device is not IPPUAddressableDevice ppuDevice)
            throw new ArgumentException("Trying to connect a non-PPU addressable device to the PPU Bus");
        
        //TODO: check to make sure not overlapping ... blah.. blah
        _devices.Add(ppuDevice);
    }

    public byte ReadByte(ushort address)
    {
        var device = _devices.FirstOrDefault(x => x.PPURange.ContainsAddress(address));

        if (device is null)
            return 0;

        return device.ReadPPU(address);
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