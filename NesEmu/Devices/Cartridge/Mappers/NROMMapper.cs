namespace NesEmu.Devices.Cartridge
{
    public class NROMMapper : Mapper
    {
        public NROMMapper(int programBanks, int characterBanks) : base(programBanks, characterBanks)
        { }

        protected override ushort GetMappedAddress(ushort suppliedAddress)
        {
            var mirrorValue = _programBanks > 1 ? 0x7FFF : 0x3FFF;

            return (ushort)(suppliedAddress & mirrorValue);
        }
    }
}