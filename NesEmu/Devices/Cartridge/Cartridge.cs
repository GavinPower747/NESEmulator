using System;
using System.Collections.Generic;
using System.IO;
using NesEmu.Core;

namespace NesEmu.Devices.Cartridge;

public class Cartridge : ICPUAddressableDevice, IPPUAddressableDevice
{
    public AddressableRange CpuRange => new(0x4020, 0xFFFF);
    public AddressableRange PPURange => new(0x0000, 0x1FFF);

    private List<byte> _programBody;
    private byte[] _characterRom;
    private readonly string _pathToRom;
    private Mapper _mapper;

    public Cartridge(string path)
    {
        _pathToRom = path;
    }

    public void Load()
    {
        using (var fileStream = new FileStream(_pathToRom, FileMode.Open))
        using (var reader = new BinaryReader(fileStream))
        {
            var header = new NesHeader(reader);
            var isValid = header.Validate();

            if (!isValid)
            {
                throw new InvalidDataException("Invalid ROM file");
            }

            _programBody = new List<byte>();

            int mirrorLowBit = header.Control1 & 1;
            int mirrorHighBit = (header.Control1 >> 3) & 1;

            byte mirrorModeByte = (byte)((mirrorHighBit << 1) | mirrorLowBit);

            var mapperIdLo = header.Control1 >> 4;
            int mapperIdHi = header.Control2 >> 4;
            var mapperId = (byte)((mapperIdHi << 4) | mapperIdLo);

            reader.ReadBytes(16);

            var programData = reader.ReadBytes(1 * 0x4000);
            _programBody.AddRange(programData);

            _characterRom =
                header.CharacterRomBanks > 0
                    ? reader.ReadBytes(header.CharacterRomBanks * 0x2000)
                    : new byte[0x2000];

            _mapper = GetMapper(mapperId, header.CharacterRomBanks, header.ProgramRomBanks);
        }
    }

    private static Mapper GetMapper(int mapperId, int characterBankCount, int programBankCount) =>
        mapperId switch
        {
            0 => new NROMMapper(programBankCount, characterBankCount),
            _ => throw new ArgumentException($"Could not find mapper for Id: {mapperId}"),
        };

    public byte ReadCpu(ushort address)
    {
        return 0;
    }

    public void WriteCpu(ushort address, byte data) { }

    public byte ReadPPU(ushort address)
    {
        return 0;
    }

    public void WritePPU(ushort address, byte data) { }
}
