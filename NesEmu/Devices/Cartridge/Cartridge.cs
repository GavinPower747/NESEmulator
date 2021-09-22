using NesEmu.Core;
using System;
using System.IO;
using System.Collections.Generic;

namespace NesEmu.Devices.Cartridge
{
    public class Cartridge : ICPUAddressableDevice, IPPUAddressableDevice
    {
        public AddressableRange CpuRange { get; }
        public AddressableRange PPURange { get; }

        private List<byte> _programBody;
        private byte[] _characterRom;
        private string _pathToRom;
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
                _programBody = new List<byte>();

                //Read the 5 byte cart header
                byte programBankCount = reader.ReadByte();
                byte characterBankCount = reader.ReadByte();
                byte control1 = reader.ReadByte();
                byte control2 = reader.ReadByte();
                byte programSize = reader.ReadByte();

                reader.ReadBytes(7); //7 empty bytes after the header, move the reader along...

                int mirrorLowBit = control1 & 1;
                int mirrorHighBit = (control1 >> 3) & 1;

                byte mirrorModeByte = (byte)((mirrorHighBit << 1) | mirrorLowBit);

                var mapperIdLo = control1 >> 4;
                int mapperIdHi = control2 >> 4;
                var mapperId = (byte)((mapperIdHi << 4) | mapperIdLo);

                reader.ReadBytes(16);

                var programData = reader.ReadBytes(1 * 0x4000);
                _programBody.AddRange(programData);

                _characterRom = characterBankCount > 0 
                    ? reader.ReadBytes(characterBankCount * 0x2000) 
                    : new byte[0x2000];

                _mapper = GetMapper(mapperId, characterBankCount, programBankCount);
            }
        }

        private Mapper GetMapper(int mapperId, int characterBankCount, int programBankCount)
        {
            switch(mapperId)
            {
                case 0: return new NROMMapper(programBankCount, characterBankCount);
                default: throw new ArgumentException($"Could not find mapper for Id: {mapperId}");
            }
        }

        public byte ReadCpu(ushort address)
        {
            return 0;
        }

        public void WriteCpu(ushort address, byte data)
        {

        }

        public byte ReadPPU(ushort address)
        {
            return 0;
        }

        public void WritePPU(ushort address, byte data)
        {

        }
    }
}