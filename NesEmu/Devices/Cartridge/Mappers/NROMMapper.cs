namespace NesEmu.Devices.Cartridge;

public class NROMMapper : Mapper
{
    public NROMMapper(int programBanks, int characterBanks) : base(programBanks, characterBanks)
    { }

    protected override ushort GetCpuMappedAddress(ushort suppliedAddress)
    {
        var mirrorValue = ProgramBanks > 1 ? 0x7FFF : 0x3FFF;

        return (ushort)(suppliedAddress & mirrorValue);
    }

    protected override ushort GetPPUMappedAddress(ushort suppliedAddress)
    {
        return suppliedAddress;
    }
}