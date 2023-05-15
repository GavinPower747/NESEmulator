using NesEmu.Devices.CPU.Instructions.Addressing;
using NesEmu.Devices.CPU.Instructions.Operations;

namespace NesEmu.Devices.CPU.Instructions;

public class Instruction
{
    public string Name { get; set; }
    public IAddressingStrategy AddressingStrategy { get; set; }
    public IOperationStrategy OperationStrategy { get; set; }
    public int Cycles { get; set; }

    public Instruction(string name, IAddressingStrategy addressingStrategy, IOperationStrategy operationStrategy, int cycles)
    {
        Name = name;
        AddressingStrategy = addressingStrategy;
        OperationStrategy = operationStrategy;
        Cycles = cycles;
    }
}