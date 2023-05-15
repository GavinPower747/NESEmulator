using NesEmu.Core;
using System;

namespace NesEmu.Devices.PPU;

///<summary>
///The Picture Processing Unit, think of it as a primitive graphics card
///</summary>
public class PPU : IClockAware, ICPUAddressableDevice, IPPUAddressableDevice
{
    public AddressableRange CpuRange => throw new NotImplementedException();

    public AddressableRange PPURange => throw new NotImplementedException();

    public PPU()
    {

    }

    public void Tick()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public byte ReadCpu(ushort address)
    {
        throw new NotImplementedException();
    }

    public byte ReadPPU(ushort address)
    {
        throw new NotImplementedException();
    }

    public void WriteCpu(ushort address, byte data)
    {
        throw new NotImplementedException();
    }

    public void WritePPU(ushort address, byte data)
    {
        throw new NotImplementedException();
    }
}