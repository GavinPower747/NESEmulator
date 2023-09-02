using System;
using System.Collections.Generic;
using System.Linq;
using NesEmu.Core;
using NesEmu.Devices.CPU.Attributes;
using NesEmu.Devices.CPU.Instructions;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Devices.CPU.Instructions.Operations;

namespace NesEmu.Devices.CPU;

internal class Cpu : IClockAware
{
    public CpuRegisters Registers;

    internal Dictionary<ushort, Instruction> OpcodeLookup;

    private IBus _bus;
    private int _cycles = 0;
    private readonly Instruction _noOpInstruction = new ("NOP", new ImpliedAddressing(), new NoOpOperation(), 2);

    internal Cpu()
    {
        Registers = new CpuRegisters();
        OpcodeLookup = new Dictionary<ushort, Instruction>();

        PopulateOpcodeLookup();
    }

    internal void ConnectBus(IBus bus) => _bus = bus;

    public void Tick()
    {
        //The traditional NES does operations in multiple cycles, there is no need for us to do it
        //like that just do everything on the last cycle
        if (_cycles == 0)
        {
            var opcode = _bus.ReadByte(Registers.ProgramCounter);

            OpcodeLookup.TryGetValue(opcode, out Instruction instruction);

            instruction ??= _noOpInstruction;

            _cycles = instruction.Cycles;
            Registers.ProgramCounter++;

            var (address, extraCycles) = instruction.AddressingStrategy.GetOperationAddress(Registers, _bus);
            var operationExtraCycles = instruction.OperationStrategy.Operate(address, Registers, _bus);

            _cycles += extraCycles + operationExtraCycles;
        }

        _cycles--;
    }

    public void Reset()
    {
        ushort address = 0xFFFC;
        Registers.ProgramCounter = _bus.ReadWord(address);

        Registers.Accumulator = 0;
        Registers.X = 0;
        Registers.Y = 0;
        Registers.StackPointer = 0xFD;
        Registers.StatusRegister = new StatusRegister(0x00);

        _cycles = 8;
    }

    internal bool OpComplete() => _cycles == 0;

    internal void Interrupt()
    {
        if (Registers.StatusRegister.InterruptDisable)
            return;

        PerformInterrupt();
    }

    internal void NonMaskableInterrupt()
    {
        PerformInterrupt();
    }

    private void PopulateOpcodeLookup()
    {
        var assembly = typeof(Cpu).Assembly;
        var opcodeTypes = assembly.GetTypes().Where(t => t.IsDefined(typeof(OpCodeAttribute), false));

        foreach (var type in opcodeTypes)
        {
            var opcodeAttributes = type.GetCustomAttributes(false).OfType<OpCodeAttribute>();

            foreach (var opcodeAttribute in opcodeAttributes)
            {
                var addressingStrategy = Activator.CreateInstance(opcodeAttribute.AddressingMode) as IAddressingStrategy;
                var operatingStrategy = Activator.CreateInstance(type) as IOperationStrategy;
                var instruction = new Instruction(operatingStrategy.Name, addressingStrategy, operatingStrategy, opcodeAttribute.Cycles);

                OpcodeLookup.Add(opcodeAttribute.OpCodeAddress, instruction);
            }
        }
    }

    private void PerformInterrupt()
    {
        _bus.Write(Registers.GetStackAddress(), (byte)((Registers.ProgramCounter >> 8) & 0x00FF));
        Registers.StackPointer--;

        _bus.Write(Registers.GetStackAddress(), (byte)(Registers.ProgramCounter & 0x00FF));
        Registers.StackPointer--;

        Registers.StatusRegister.Break = true;
        Registers.StatusRegister.InterruptDisable = true;
        _bus.Write(Registers.GetStackAddress(), Registers.StatusRegister);
        Registers.StackPointer--;

        Registers.ProgramCounter = _bus.ReadWord(0xFFFA);

        _cycles = 8;
    }
}