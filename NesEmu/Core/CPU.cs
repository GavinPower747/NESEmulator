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
            { 0x01, new Instruction( "ORA", new IndirectXAddressing(), new OrAccumulatorOperation(), 6)}
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
                var data = _bus.Read(addressInfo.address);
                var operationExtraCycles = instruction.OperationStrategy.Operate(data, Registers, _bus);

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