using System.Reflection;
using Moq;
using NesEmu.Core;
using NesEmu.Devices;
using NUnit.Framework;

namespace NesEmu.Tests.Devices
{
    [TestFixture]
    public class RAMTests
    {
        private Mock<IBus> _bus;

        [SetUp]
        public void Setup()
        {
            _bus = new Mock<IBus>();
        }

        [Test]
        public void Read_Returns_Correct_ValueBelow2KB()
        {
            ushort address = 0x0010;
            byte memoryVal = 0xFF;
            var ramData = new byte[2048];

            ramData[address] = memoryVal;

            var ram = new Ram();
            SetRamData(ram, ramData);

            var data = ram.Read(address);

            Assert.That(data, Is.EqualTo(memoryVal));
        }

        [Test]
        public void Read_Returns_Correct_ValueAbove2KB()
        {
            ushort address = 3000;
            byte memoryVal = 0xFF;
            var ramData = new byte[2048];

            ramData[address - 2048] = memoryVal;

            var ram = new Ram();
            SetRamData(ram, ramData);

            var data = ram.Read(address);

            Assert.That(data, Is.EqualTo(memoryVal));
        }

        [Test]
        public void Write_Sets_CorrectLocation_Below2KB()
        {
            ushort address = 0x0010;
            byte memoryVal = 0xFF;

            var ram = new Ram();
            ram.Write(address, memoryVal);

            var data = ram.Read(address);

            Assert.That(data, Is.EqualTo(memoryVal));
        }

        [Test]
        public void Write_Sets_CorrectLocation_Above2KB()
        {
            ushort address = 3000;
            byte memoryVal = 0xFF;

            var ram = new Ram();
            ram.Write(address, memoryVal);

            var data = ram.Read((ushort)(address - 2048));

            Assert.That(data, Is.EqualTo(memoryVal));
        }

        private void SetRamData(Ram ram, byte[] data)
        {
            var type = ram.GetType();
            var field = type.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);

            field.SetValue(ram, data);
        }
    }
}