using System.IO;
using System.Collections.Generic;

namespace NesEmu.Devices.Cartridge
{
    public class Cartridge
    {
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
                List<byte> romBody = new List<byte>();

                //Read the 5 byte cart header
                byte programBankCount = reader.ReadByte();
                byte characterBankCount = reader.ReadByte();
                byte control1 = reader.ReadByte();
                byte control2 = reader.ReadByte();
                byte programSize = reader.ReadByte();

                reader.ReadBytes(7); //7 empty bytes after the header, move the reader along...

                
            }
        }
    }
}