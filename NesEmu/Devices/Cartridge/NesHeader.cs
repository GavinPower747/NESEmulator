using System;
using System.IO;

namespace NesEmu.Devices.Cartridge;

public readonly struct NesHeader
{
    /// <summary>
    /// The 4KB "magic number" that should be present at the start of every NES file
    /// </summary>
    public readonly byte[] FileSignature;

    /// <summary>
    /// The number of 16KB program ROM banks
    /// </summary>
    /// <remarks>
    /// This is the number of 16KB banks that are present in the ROM image. 
    /// The smallest possible value is 1, which indicates 16KB of program ROM. 
    /// The largest possible value is 255, which indicates 4MB of program ROM.
    /// </remarks>
    public readonly byte ProgramRomBanks;

    /// <summary>
    /// The number of 8KB character ROM banks
    /// </summary>
    /// <remarks>
    /// This is the number of 8KB banks that are present in the ROM image.
    /// The smallest possible value is 0, which indicates 8KB of character ROM.
    /// The largest possible value is 255, which indicates 2MB of character ROM.
    /// </remarks>
    public readonly byte CharacterRomBanks;

    /// <summary>
    /// The control byte 1
    /// </summary>
    /// <remarks>
    /// This byte contains a number of flags that control the ROM image.
    /// </remarks>
    /// <see cref="https://wiki.nesdev.com/w/index.php/INES#Flags_6"/>
    /// <see cref="https://wiki.nesdev.com/w/index.php/Mapper"/>
    public readonly byte Control1;

    /// <summary>
    /// The control byte 2
    /// </summary>
    /// <remarks>
    /// This byte contains a number of flags that control the ROM image.
    /// </remarks>
    /// <see cref="https://wiki.nesdev.com/w/index.php/INES#Flags_7"/>
    /// <see cref="https://wiki.nesdev.com/w/index.php/Mapper"/>
    public readonly byte Control2;

    public readonly byte ProgramRamSize;
    public readonly byte TvSystem;
    public readonly byte TvSystem2;

    private static Span<byte> ValidFileSignature => new byte[] { 0x4E, 0x45, 0x53, 0x1A };

    public NesHeader(BinaryReader reader)
    {
        FileSignature = reader.ReadBytes(4);
        ProgramRomBanks = reader.ReadByte();
        CharacterRomBanks = reader.ReadByte();
        Control1 = reader.ReadByte();
        Control2 = reader.ReadByte();
        ProgramRamSize = reader.ReadByte();
        TvSystem = reader.ReadByte();
        TvSystem2 = reader.ReadByte();

        reader.ReadBytes(5); //5 empty bytes after the header, move the reader along...
    }

    public readonly bool Validate() => FileSignature.AsSpan().SequenceEqual(ValidFileSignature);
}
