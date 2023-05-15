using System;

namespace NesEmu.Devices.CPU.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class OpCodeAttribute : Attribute
{
    public ushort OpCodeAddress { get; set; }
    public Type AddressingMode { get; set; }
    public int Cycles { get; set; }
}