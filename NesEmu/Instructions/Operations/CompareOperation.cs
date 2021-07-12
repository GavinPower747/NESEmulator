using System;
using NesEmu.Core;
using NesEmu.Extensions;

namespace NesEmu.Instructions.Operations
{
    public class CompareOperation : IOperationStrategy
    {
        private Func<CPURegisters, byte> _selector;

        public CompareOperation(Func<CPURegisters, byte> selector)
        {
            _selector = selector;
        }

        public int Operate(ushort address, CPURegisters registers, IBus bus)
        {
            ushort memoryVal = bus.ReadByte(address);
            ushort registerVal = _selector(registers);
            ushort comparedVal = (ushort)(registerVal - memoryVal);

            registers.StatusRegister.Carry = comparedVal >= 0;
            registers.StatusRegister.Zero = comparedVal == 0;
            registers.StatusRegister.Negative = (comparedVal & 1 << 7) > 0;

            return 0;
        }
    }
}