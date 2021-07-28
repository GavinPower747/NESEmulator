using System.Collections.Generic;
using NesEmu.Core;
using NesEmu.Devices.CPU.Instructions;
using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Devices.CPU.Instructions.Operations;

namespace NesEmu.Devices.CPU
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
            { 0xB8, new Instruction( "CLV", new ImpliedAddressing(), new ClearOverflowFlagOperation(), 2)},
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
            { 0x71, new Instruction( "ADC", new IndirectYAddressing(), new AddWithCarryOperation(), 5)},
            { 0x0A, new Instruction( "ASL", new ImpliedAddressing(), new ArithmeticShiftLeftAccumulatorOperation(), 2)},
            { 0x06, new Instruction( "ASL", new ZeroPageAddressing(), new ArithmeticShiftLeftOperation(), 5)},
            { 0x16, new Instruction( "ASL", new ZeroPageXOffsetAddressing(), new ArithmeticShiftLeftOperation(), 6)},
            { 0x0E, new Instruction( "ASL", new AbsoluteAddressing(), new ArithmeticShiftLeftOperation(), 6)},
            { 0x1E, new Instruction( "ASL", new AbsoluteXOffsetAddressing(), new ArithmeticShiftLeftOperation(), 7)},
            { 0x2A, new Instruction( "ROL", new ImpliedAddressing(), new RotateLeftAccumulatorOperation(), 2)},
            { 0x26, new Instruction( "ROL", new ZeroPageAddressing(), new RotateLeftOperation(), 5)},
            { 0x36, new Instruction( "ROL", new ZeroPageXOffsetAddressing(), new RotateLeftOperation(), 6)},
            { 0x2E, new Instruction( "ROL", new AbsoluteAddressing(), new RotateLeftOperation(), 6)},
            { 0x3E, new Instruction( "ROL", new AbsoluteXOffsetAddressing(), new RotateLeftOperation(), 7)},
            { 0x90, new Instruction( "BCC", new RelativeAddressing(), new BranchOperation(reg => !reg.StatusRegister.Carry), 2)},
            { 0xB0, new Instruction( "BCS", new RelativeAddressing(), new BranchOperation(reg => reg.StatusRegister.Carry), 2)},
            { 0xF0, new Instruction( "BEQ", new RelativeAddressing(), new BranchOperation(reg => reg.StatusRegister.Zero), 2)},
            { 0x30, new Instruction( "BMI", new RelativeAddressing(), new BranchOperation(reg => reg.StatusRegister.Negative), 2)},
            { 0xD0, new Instruction( "BNE", new RelativeAddressing(), new BranchOperation(reg => !reg.StatusRegister.Zero), 2)},
            { 0x10, new Instruction( "BPL", new RelativeAddressing(), new BranchOperation(reg => !reg.StatusRegister.Negative), 2)},
            { 0x50, new Instruction( "BVC", new RelativeAddressing(), new BranchOperation(reg => !reg.StatusRegister.Overflow), 2)},
            { 0x70, new Instruction( "BVS", new RelativeAddressing(), new BranchOperation(reg => reg.StatusRegister.Overflow), 2)},
            { 0x24, new Instruction( "BIT", new ZeroPageAddressing(), new BitTestOperation(), 3)},
            { 0x2C, new Instruction( "BIT", new AbsoluteAddressing(), new BitTestOperation(), 4)},
            { 0xC9, new Instruction( "CMP", new ImmediateAddressing(), new CompareOperation(r => r.Accumulator), 2)},
            { 0xC5, new Instruction( "CMP", new ZeroPageAddressing(), new CompareOperation(r => r.Accumulator), 3)},
            { 0xD5, new Instruction( "CMP", new ZeroPageXOffsetAddressing(), new CompareOperation(r => r.Accumulator), 4)},
            { 0xCD, new Instruction( "CMP", new AbsoluteAddressing(), new CompareOperation(r => r.Accumulator), 4)},
            { 0xDD, new Instruction( "CMP", new AbsoluteXOffsetAddressing(), new CompareOperation(r => r.Accumulator), 4)}, 
            { 0xD9, new Instruction( "CMP", new AbsoluteYOffsetAddressing(), new CompareOperation(r => r.Accumulator), 4)},
            { 0xC1, new Instruction( "CMP", new IndirectXAddressing(), new CompareOperation(r => r.Accumulator), 6)},
            { 0xD1, new Instruction( "CMP", new IndirectYAddressing(), new CompareOperation(r => r.Accumulator), 5)},
            { 0xE0, new Instruction( "CPX", new ImmediateAddressing(), new CompareOperation(r => r.X), 2)},
            { 0xE4, new Instruction( "CPX", new ZeroPageAddressing(), new CompareOperation(r => r.X), 3)},
            { 0xEC, new Instruction( "CPX", new AbsoluteAddressing(), new CompareOperation(r => r.X), 4)},
            { 0xC0, new Instruction( "CPY", new ImmediateAddressing(), new CompareOperation(r => r.Y), 2)},
            { 0xC4, new Instruction( "CPY", new ZeroPageAddressing(), new CompareOperation(r => r.Y), 3)},
            { 0xCC, new Instruction( "CPY", new AbsoluteAddressing(), new CompareOperation(r => r.Y), 4)},
            { 0x49, new Instruction( "EOR", new ImmediateAddressing(), new ExclusiveOROperation(), 2)},
            { 0x45, new Instruction( "EOR", new ZeroPageAddressing(), new ExclusiveOROperation(), 3)},
            { 0x55, new Instruction( "EOR", new ZeroPageXOffsetAddressing(), new ExclusiveOROperation(), 4)},
            { 0x4D, new Instruction( "EOR", new AbsoluteAddressing(), new ExclusiveOROperation(), 4)},
            { 0x5D, new Instruction( "EOR", new AbsoluteXOffsetAddressing(), new ExclusiveOROperation(), 4)},
            { 0x59, new Instruction( "EOR", new AbsoluteYOffsetAddressing(), new ExclusiveOROperation(), 4)},
            { 0x41, new Instruction( "EOR", new IndirectXAddressing(), new ExclusiveOROperation(), 6)},
            { 0x51, new Instruction( "EOR", new IndirectYAddressing(), new ExclusiveOROperation(), 5)},
            { 0x4A, new Instruction( "LSR", new ImpliedAddressing(), new LogicalShiftRightAccumulatorOperation(), 2)},
            { 0x46, new Instruction( "LSR", new ZeroPageAddressing(), new LogicalShiftRightMemoryOperation(), 5)},
            { 0x56, new Instruction( "LSR", new ZeroPageXOffsetAddressing(), new LogicalShiftRightMemoryOperation(), 6)},
            { 0x4E, new Instruction( "LSR", new AbsoluteAddressing(), new LogicalShiftRightMemoryOperation(), 6)},
            { 0x5E, new Instruction( "LSR", new AbsoluteXOffsetAddressing(), new LogicalShiftRightMemoryOperation(), 7)},
            { 0x6A, new Instruction( "ROR", new ImpliedAddressing(), new RotateRightAccumulatorOperation(), 2)},
            { 0x66, new Instruction( "ROR", new ZeroPageAddressing(), new RotateRightOperation(), 5)},
            { 0x76, new Instruction( "ROR", new ZeroPageXOffsetAddressing(), new RotateRightOperation(), 6)},
            { 0x6E, new Instruction( "ROR", new AbsoluteAddressing(), new RotateRightOperation(), 6)},
            { 0x7E, new Instruction( "ROR", new AbsoluteXOffsetAddressing(), new RotateRightOperation(), 7)},
            { 0x40, new Instruction( "RTI", new ImpliedAddressing(), new ReturnFromInterrupt(), 6)},
            { 0x60, new Instruction( "RTS", new ImpliedAddressing(), new ReturnFromSubroutine(), 6)},
            { 0xE9, new Instruction( "SBC", new ImmediateAddressing(), new SubtractWithCarryOperation(), 2)},
            { 0xE5, new Instruction( "SBC", new ZeroPageAddressing(), new SubtractWithCarryOperation(), 3)},
            { 0xF5, new Instruction( "SBC", new ZeroPageXOffsetAddressing(), new SubtractWithCarryOperation(), 4)},
            { 0xED, new Instruction( "SBC", new AbsoluteAddressing(), new SubtractWithCarryOperation(), 4)},
            { 0xFD, new Instruction( "SBC", new AbsoluteXOffsetAddressing(), new SubtractWithCarryOperation(), 4)},
            { 0xF9, new Instruction( "SBC", new AbsoluteYOffsetAddressing(), new SubtractWithCarryOperation(), 4)},
            { 0xE1, new Instruction( "SBC", new IndirectXAddressing(), new SubtractWithCarryOperation(), 6)},
            { 0xF1, new Instruction( "SBC", new IndirectYAddressing(), new SubtractWithCarryOperation(), 5)}
        };

        private readonly Instruction _noOpInstruction = new Instruction("NOP", new ImpliedAddressing(), new NoOpOperation(), 2);

        public CPU()
        {
            Registers = new CPURegisters();
        }

        public void ConnectBus(IBus bus) => _bus = bus;

        public void Tick()
        {
            //The traditional NES does operations in multiple cycles, there is no need for us to do it
            //like that just do everything on the last cycle
            if(_cycles == 0)
            {
                var opcode = _bus.ReadByte(Registers.ProgramCounter);
                Instruction instruction = null;

                _opcodeLookup.TryGetValue(opcode, out instruction);

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
            ushort address = 0xFFFC;
            Registers.ProgramCounter = _bus.ReadWord(address);

            Registers.Accumulator = 0;
            Registers.X = 0;
            Registers.Y = 0;
            Registers.StackPointer = 0xFD;
            Registers.StatusRegister = new StatusRegister(0x00);

            _cycles = 8;
        }

        public bool OpComplete() => _cycles == 0;

        public void Interrupt()
        {
            if(Registers.StatusRegister.InterruptDisable)
                return;
        }

        public Dictionary<ushort, string> GetDisassembly(ushort start, ushort end)
        {
            var values = new Dictionary<ushort, string>();
            var address = start;

            while(address < end)
            {
                Instruction instruction = null;
                var opcode = _bus.ReadByte(address);
                var lineAddress = address;
                var lineString = string.Empty;

                if(opcode == 0)
                {
                    address++;
                    continue;
                }

                _opcodeLookup.TryGetValue(opcode, out instruction);

                if(instruction is null)
                    instruction = _noOpInstruction;

                address++;

                lineString = instruction.Name + "  ";

                switch (instruction.AddressingStrategy)
                {
                    case ImpliedAddressing _:
                        lineString += " {IMP}";
                        break;
                    case ImmediateAddressing _:
                        {
                            var value = _bus.ReadByte(address);
                            address++;
                            lineString += "#$" + value.ToString("X2") + " {IMM}";
                            break;
                        }

                    case ZeroPageAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + lo.ToString("X2") + " {ZP0}";
                            break;
                        }

                    case ZeroPageXOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + lo.ToString("X2") + ", X {ZPX}";
                            break;
                        }

                    case ZeroPageYOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + lo.ToString("X2") + ", Y {ZPY}";
                            break;
                        }

                    case IndirectXAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "($" + lo.ToString("X2") + ", X) {IZX}";
                            break;
                        }

                    case IndirectYAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            lineString += "($" + lo.ToString("X2") + "), Y {IZY}";
                            break;
                        }

                    case AbsoluteAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + ((ushort)(hi << 8) | lo).ToString("X4") + " {ABS}";
                            break;
                        }

                    case AbsoluteXOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + ((ushort)((hi << 8) | lo)).ToString("X4") + ", X {ABX}";
                            break;
                        }

                    case AbsoluteYOffsetAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + ((ushort)((hi << 8) | lo)).ToString("X4") + ", Y {ABY}";
                            break;
                        }

                    case IndirectAddressing _:
                        {
                            var lo = _bus.ReadByte(address);
                            address++;
                            var hi = _bus.ReadByte(address);
                            address++;
                            lineString += "($" + ((ushort)(hi << 8) | lo).ToString("X4") + ") {IND}";
                            break;
                        }

                    case RelativeAddressing _:
                        {
                            var value = _bus.ReadByte(address);
                            address++;
                            lineString += "$" + value.ToString("X2") + " [$" + ((ushort)address + value).ToString("X4") + "] {REL}";
                            break;
                        }
                }

                if(!values.ContainsKey(lineAddress))
                    values.Add(lineAddress, lineString);
            }

            return values;
        }
    }
}