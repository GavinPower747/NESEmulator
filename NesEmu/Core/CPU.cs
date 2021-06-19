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
            { 0xBC, new Instruction( "LDY", new AbsoluteXOffsetAddressing(), new LoadYRegisterOperation(), 4)},
            { 0x48, new Instruction( "PHA", new ImpliedAddressing(), new PushAccumulatorOperation(), 3)},
            { 0x08, new Instruction( "PHP", new ImpliedAddressing(), new PushProcessorStatusOperation(), 3)},
            { 0x68, new Instruction( "PLA", new ImpliedAddressing(), new PullAccumulatorOperation(), 4)},
            { 0x28, new Instruction( "PLP", new ImpliedAddressing(), new PullStatusOperation(), 4)},
            { 0xAA, new Instruction( "TAX", new ImpliedAddressing(), new TransferAccumulatorX(), 2)},
            { 0xA8, new Instruction( "TAY", new ImpliedAddressing(), new TransferAccumulatorY(), 2)},
            { 0xBA, new Instruction( "TXS", new ImpliedAddressing(), new TransferStackXOperation(), 2)},
            { 0x8A, new Instruction( "TXA", new ImpliedAddressing(), new TransferXAccumulator(), 2)},
            { 0x9A, new Instruction( "TSX", new ImpliedAddressing(), new TransferXStackOperation(), 2)},
            { 0x98, new Instruction( "TYA", new ImpliedAddressing(), new TransferYAccumulator(), 2)},
            { 0x78, new Instruction( "SEI", new ImpliedAddressing(), new SetInteruptDisableOperation(), 2)},
            { 0x85, new Instruction( "STA", new ZeroPageAddressing(), new StoreAccumulatorOperation(), 3)},
            { 0x95, new Instruction( "STA", new ZeroPageXOffsetAddressing(), new StoreAccumulatorOperation(), 4)},
            { 0x8D, new Instruction( "STA", new AbsoluteAddressing(), new StoreAccumulatorOperation(), 4)},
            { 0x9D, new Instruction( "STA", new AbsoluteXOffsetAddressing(), new StoreAccumulatorOperation(), 5)},
            { 0x99, new Instruction( "STA", new AbsoluteYOffsetAddressing(), new StoreAccumulatorOperation(), 5)},
            { 0x81, new Instruction( "STA", new IndirectXAddressing(), new StoreAccumulatorOperation(), 6)},
            { 0x91, new Instruction( "STA", new IndirectYAddressing(), new StoreAccumulatorOperation(), 6)},
            { 0x86, new Instruction( "STX", new ZeroPageAddressing(), new StoreXRegisterOperation(), 3)},
            { 0x96, new Instruction( "STX", new ZeroPageYOffsetAddressing(), new StoreXRegisterOperation(), 4)},
            { 0x8E, new Instruction( "STX", new AbsoluteAddressing(), new StoreXRegisterOperation(), 4)},
            { 0x84, new Instruction( "STY", new ZeroPageAddressing(), new StoreYRegisterOperation(), 3)},
            { 0x94, new Instruction( "STY", new ZeroPageXOffsetAddressing(), new StoreYRegisterOperation(), 4)},
            { 0x8C, new Instruction( "STY", new AbsoluteAddressing(), new StoreYRegisterOperation(), 4)},
            { 0xF8, new Instruction( "SED", new ImpliedAddressing(), new SetDecimalFlagOperation(), 2)},
            { 0x38, new Instruction( "SEC", new ImpliedAddressing(), new SetCarryFlagOperation(), 2)},
            { 0x69, new Instruction( "ADC", new ImmediateAddressing(), new AddWithCarryOperation(), 2)},
            { 0x65, new Instruction( "ADC", new ZeroPageAddressing(), new AddWithCarryOperation(), 3)},
            { 0x75, new Instruction( "ADC", new ZeroPageXOffsetAddressing(), new AddWithCarryOperation(), 4)},
            { 0x6D, new Instruction( "ADC", new AbsoluteAddressing(), new AddWithCarryOperation(), 4)},
            { 0x7D, new Instruction( "ADC", new AbsoluteXOffsetAddressing(), new AddWithCarryOperation(), 4)},
            { 0x79, new Instruction( "ADC", new AbsoluteYOffsetAddressing(), new AddWithCarryOperation(), 4)},
            { 0x61, new Instruction( "ADC", new IndirectXAddressing(), new AddWithCarryOperation(), 6)},
            { 0x71, new Instruction( "ADC", new IndirectYAddressing(), new AddWithCarryOperation(), 5)}
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