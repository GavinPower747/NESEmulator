using System.Collections.Generic;
using NesEmu.Instructions;
using NesEmu.Instructions.Operations;
using NesEmu.Instructions.Addressing;

namespace NesEmu.Core
{
    public class CPU : IClockAware
    {
        public CPURegisters Registers;

        private IBus _bus;
        private int _cycles = 0;

        private Dictionary<ushort, Instruction> _opcodeLookup => new Dictionary<ushort, Instruction>()
        {
            { 0x00, new Instruction( "BRK", new ImpliedAddressing(), new BreakOperation(), 7) },
            { 0x09, new Instruction( "ORA", new ImmediateAddressing(), new LogicalOrOperation(), 2)},
            { 0x05, new Instruction( "ORA", new ZeroPageAddressing(), new LogicalOrOperation(), 3)},
            { 0x15, new Instruction( "ORA", new ZeroPageXOffsetAddressing(), new LogicalOrOperation(), 4)},
            { 0x0D, new Instruction( "ORA", new AbsoluteAddressing(), new LogicalOrOperation(), 4)},
            { 0x1D, new Instruction( "ORA", new AbsoluteXOffsetAddressing(), new LogicalOrOperation(), 4)},
            { 0x19, new Instruction( "ORA", new AbsoluteYOffsetAddressing(), new LogicalOrOperation(), 4)},
            { 0x01, new Instruction( "ORA", new IndirectXAddressing(), new LogicalOrOperation(), 6)},
            { 0x11, new Instruction( "ORA", new IndirectYAddressing(), new LogicalOrOperation(), 5)},
            { 0x29, new Instruction( "AND", new ImmediateAddressing(), new LogicalAndOperation(), 2)},
            { 0x25, new Instruction( "AND", new ZeroPageAddressing(), new LogicalAndOperation(), 3)},
            { 0x35, new Instruction( "AND", new ZeroPageXOffsetAddressing(), new LogicalAndOperation(), 4)},
            { 0x2D, new Instruction( "AND", new AbsoluteAddressing(), new LogicalAndOperation(), 4)},
            { 0x3D, new Instruction( "AND", new AbsoluteXOffsetAddressing(), new LogicalAndOperation(), 4)},
            { 0x39, new Instruction( "AND", new AbsoluteYOffsetAddressing(), new LogicalAndOperation(), 4)},
            { 0x21, new Instruction( "AND", new IndirectXAddressing(), new LogicalAndOperation(), 6)},
            { 0x31, new Instruction( "AND", new IndirectYAddressing(), new LogicalAndOperation(), 5)},
            { 0x18, new Instruction( "CLC", new ImpliedAddressing(), new ClearCarryFlagOperation(), 2)},
            { 0xD8, new Instruction( "CLD", new ImpliedAddressing(), new ClearDecimalModeOperation(), 2)},
            { 0x58, new Instruction( "CLI", new ImpliedAddressing(), new ClearInterruptDisableOperation(), 2)},
            { 0xB8, new Instruction( "CLO", new ImpliedAddressing(), new ClearOverflowFlagOperation(), 2)},
            { 0xC6, new Instruction( "DEC", new ZeroPageAddressing(), new DecrementMemoryOperation(), 5)},
            { 0xD6, new Instruction( "DEC", new ZeroPageXOffsetAddressing(), new DecrementMemoryOperation(), 6)},
            { 0xCE, new Instruction( "DEC", new AbsoluteAddressing(), new DecrementMemoryOperation(), 6)},
            { 0xDE, new Instruction( "DEC", new AbsoluteXOffsetAddressing(), new DecrementMemoryOperation(), 7)},
            { 0xCA, new Instruction( "DEX", new ImpliedAddressing(), new DecrementXRegisterOperation(), 2)},
            { 0x88, new Instruction( "DEY", new ImpliedAddressing(), new DecrementYRegisterOperation(), 2)},
            { 0xE6, new Instruction( "INC", new ZeroPageAddressing(), new IncrementMemoryOperation(), 5)},
            { 0xF6, new Instruction( "INC", new ZeroPageXOffsetAddressing(), new IncrementMemoryOperation(), 6)},
            { 0xEE, new Instruction( "INC", new AbsoluteAddressing(), new IncrementMemoryOperation(), 6)},
            { 0xFE, new Instruction( "INC", new AbsoluteXOffsetAddressing(), new IncrementMemoryOperation(), 7)},
            { 0xE8, new Instruction( "INX", new ImpliedAddressing(), new IncrementXRegisterOperation(), 2)},
            { 0xC8, new Instruction( "INY", new ImpliedAddressing(), new IncrementYRegisterOperation(), 2)},
            { 0x4C, new Instruction( "JMP", new AbsoluteAddressing(), new JumpOperation(), 3)},
            { 0x6C, new Instruction( "JMP", new IndirectAddressing(), new JumpOperation(), 5)},
            { 0x20, new Instruction( "JSR", new AbsoluteAddressing(), new JumpToSubroutineOperation(), 6)},
            { 0xA9, new Instruction( "LDA", new ImmediateAddressing(), new LoadAccumulatorOperation(), 2)},
            { 0xA5, new Instruction( "LDA", new ZeroPageAddressing(), new LoadAccumulatorOperation(), 3)},
            { 0xB5, new Instruction( "LDA", new ZeroPageXOffsetAddressing(), new LoadAccumulatorOperation(), 4)},
            { 0xAD, new Instruction( "LDA", new AbsoluteAddressing(), new LoadAccumulatorOperation(), 4)},
            { 0xBD, new Instruction( "LDA", new AbsoluteXOffsetAddressing(), new LoadAccumulatorOperation(), 4)},
            { 0xB9, new Instruction( "LDA", new AbsoluteYOffsetAddressing(), new LoadAccumulatorOperation(), 4)},
            { 0xA1, new Instruction( "LDA", new IndirectXAddressing(), new LoadAccumulatorOperation(), 6)},
            { 0xB1, new Instruction( "LDA", new IndirectYAddressing(), new LoadAccumulatorOperation(), 5)},
            { 0xA2, new Instruction( "LDX", new ImmediateAddressing(), new LoadXRegisterOperation(), 2)},
            { 0xA6, new Instruction( "LDX", new ZeroPageAddressing(), new LoadXRegisterOperation(), 3)},
            { 0xB6, new Instruction( "LDX", new ZeroPageYOffsetAddressing(), new LoadXRegisterOperation(), 4)},
            { 0xAE, new Instruction( "LDX", new AbsoluteAddressing(), new LoadXRegisterOperation(), 4)},
            { 0xBE, new Instruction( "LDX", new AbsoluteYOffsetAddressing(), new LoadXRegisterOperation(), 4)},
            { 0xA0, new Instruction( "LDY", new ImmediateAddressing(), new LoadYRegisterOperation(), 2)},
            { 0xA4, new Instruction( "LDY", new ZeroPageAddressing(), new LoadYRegisterOperation(), 3)},
            { 0xB4, new Instruction( "LDY", new ZeroPageXOffsetAddressing(), new LoadYRegisterOperation(), 4)},
            { 0xAC, new Instruction( "LDY", new AbsoluteAddressing(), new LoadYRegisterOperation(), 4)},
            { 0xBC, new Instruction( "LDY", new AbsoluteXOffsetAddressing(), new LoadYRegisterOperation(), 4)}
        };
        private readonly Instruction _noOpInstruction = new Instruction("NOP", new ImpliedAddressing(), new NoOpOperation(), 0);

        public void ConnectBus(IBus bus) => _bus = bus;

        public void Tick()
        {
            //The traditional NES does operations in multiple cycles, there is no need for us to do it
            //like that just do everything on the last cycle
            if(_cycles == 0)
            {
                var opcode = _bus.Read(Registers.ProgramCounter);
                var instruction = _opcodeLookup[opcode];

                if(instruction is null)
                    instruction = _noOpInstruction;

                _cycles = instruction.Cycles;
                Registers.ProgramCounter++;

                var addressInfo = instruction.AddressingStrategy.GetOperationAddress(Registers, _bus);
                var operationExtraCycles = instruction.OperationStrategy.Operate(addressInfo.address, Registers, _bus);

                _cycles += addressInfo.extraCycles + operationExtraCycles;
            }

            _cycles--;
        }

        public void Reset()
        {

        }

        public void Interrupt()
        {
            if(Registers.StatusRegister.HasFlag(StatusRegister.InterruptDisable))
                return;
        }
    }
}