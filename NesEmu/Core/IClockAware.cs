namespace NesEmu.Core
{
    internal interface IClockAware
    {
        void Tick();
        void Reset();
    }
}