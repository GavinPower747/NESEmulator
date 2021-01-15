namespace NesEmu.Core
{
    public interface IClockAware
    {
        void Tick();
        void Reset();
    }
}