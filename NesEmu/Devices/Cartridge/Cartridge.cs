using System;
using System.IO;
using System.Collections.Generic;

namespace NesEmu.Devices.Cartridge
{
    public class Cartridge
    {
        private List<byte> _programBody;
        private byte[] _characterRom;
        private string _pathToRom;

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

                var programData = reader.ReadBytes(programBankCount * 0x4000);
                _programBody.AddRange(programData);

                if(characterBankCount == 0)
                {
                    _characterRom = new byte[0x2000];
                }                
                else
                {
                    _characterRom = reader.ReadBytes(characterBankCount * 0x2000);
                }

                OutputProgramData(programBankCount, characterBankCount, control1, control2, programSize, mapperId, mirrorModeByte);
            }
        }

        private void OutputProgramData(byte programBankCount, byte characterBankCount, byte control1, byte control2, byte programSize, byte mapperId, byte mirrorModeByte)
        {
            Console.WriteLine($"ProgramBankCount: {programBankCount.ToString()}");
            Console.WriteLine($"CharacterBankCount: {characterBankCount.ToString()}");
            Console.WriteLine($"control1: {Convert.ToString(control1, 2).PadLeft(8, '0')}");
            Console.WriteLine($"control2: {Convert.ToString(control2, 2).PadLeft(8, '0')}");
            Console.WriteLine($"ProgramSize: {programSize.ToString()}");
            Console.WriteLine($"MapperId: {mapperId.ToString()}");
            Console.WriteLine($"Mirror Mode: {mirrorModeByte}");
            Console.WriteLine($"Program Data: {BitConverter.ToString(_programBody.ToArray())}");
        }
    }
}