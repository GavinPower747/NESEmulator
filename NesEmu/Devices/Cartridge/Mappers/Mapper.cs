namespace NesEmu.Devices.Cartridge;

public abstract class Mapper
{
    protected readonly int ProgramBanks;
    protected readonly int CharacterBanks;

    public Mapper(int programBanks, int characterBanks)
    {
        ProgramBanks = programBanks;
        CharacterBanks = characterBanks;
    }

    public abstract ushort GetCpuMappedAddress(ushort suppliedAddress);
    public abstract ushort GetPPUMappedAddress(ushort suppliedAddress);
}