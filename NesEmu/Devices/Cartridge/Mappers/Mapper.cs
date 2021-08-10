namespace NesEmu.Devices.Cartridge
{
    public abstract class Mapper
    {
        protected readonly int _programBanks;
        protected readonly int _characterBanks;

        public Mapper(int programBanks, int characterBanks)
        {
            _programBanks = programBanks;
            _characterBanks = characterBanks;
        }

        protected abstract ushort GetMappedAddress(ushort suppliedAddress);
    }
}