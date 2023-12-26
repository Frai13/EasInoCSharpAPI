using EasInoAPI.Configuration;
using System.IO.Ports;
using System.Threading;

namespace EasInoNetCoreUnitTests
{
    public class TestsConfiguration
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSerialConfigConstructor()
        {
            SerialComConfiguration sc = new SerialComConfiguration("COM1");
            Assert.That(sc.ComType == GenericConfiguration.CommunicationType.SERIAL);
            Assert.That(sc.ComPort == "COM1");
            Assert.That(sc.Timeout == 2000);
            Assert.That(sc.BaudRate == 9600);
            Assert.That(sc.Parity == Parity.None);
            Assert.That(sc.DataBits == 8);
            Assert.That(sc.StopBits == StopBits.One);
        }

        [Test]
        public void TestSerialConfigEnumerableConstructor()
        {
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(100, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(-1, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(100, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(-1, "COM1", 9600, Parity.None, 6, StopBits.One);

            Helper.SCEnumerableConstructor(0, "COM1", 110, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 300, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 1200, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 2400, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 4800, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 14400, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 19200, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 38400, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 57600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 115200, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 128000, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 256000, Parity.None, 6, StopBits.One);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var a = new SerialComConfiguration(new List<string>() { "SERIAL", "0", "COM1", "1", "NONE", "6", "One" });
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var a = new SerialComConfiguration(new List<string>() { "SERIAL", "0", "COM1", "-1", "NONE", "6", "One" });
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var a = new SerialComConfiguration(new List<string>() { "SERIAL", "0", "COM1", "9599", "NONE", "6", "One" });
            });

            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.Odd, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.Even, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.Mark, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.Space, 6, StopBits.One);

            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 5, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 7, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 8, StopBits.One);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var a = new SerialComConfiguration(new List<string>() { "SERIAL", "0", "COM1", "1", "NONE", "4", "One" });
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var a = new SerialComConfiguration(new List<string>() { "SERIAL", "0", "COM1", "1", "NONE", "9", "One" });
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var a = new SerialComConfiguration(new List<string>() { "SERIAL", "0", "COM1", "1", "NONE", "0", "One" });
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var a = new SerialComConfiguration(new List<string>() { "SERIAL", "0", "COM1", "1", "NONE", "-1", "One" });
            });

            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.None);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.Two);
            Helper.SCEnumerableConstructor(0, "COM1", 9600, Parity.None, 6, StopBits.OnePointFive);
        }

        [Test]
        public void TestGenericConfigSerialize()
        {
            Helper.GCSerialize(GenericConfiguration.CommunicationType.NONE, 0);
            Helper.GCSerialize(GenericConfiguration.CommunicationType.NONE, 100);
            Helper.GCSerialize(GenericConfiguration.CommunicationType.NONE, -1);
            Helper.GCSerialize(GenericConfiguration.CommunicationType.SERIAL, 0);
            Helper.GCSerialize(GenericConfiguration.CommunicationType.SERIAL, 100);
            Helper.GCSerialize(GenericConfiguration.CommunicationType.SERIAL, -1);
        }

        [Test]
        public void TestSerialConfigSerialize()
        {
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(100, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(-1, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(100, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(-1, "COM1", 9600, Parity.None, 6, StopBits.One);

            Helper.SCSerialize(0, "COM1", 110, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 300, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 1200, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 2400, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 4800, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 14400, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 19200, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 38400, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 57600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 115200, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 128000, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 256000, Parity.None, 6, StopBits.One);
            Assert.Throws<ArgumentOutOfRangeException>(() => { var a = new SerialComConfiguration() { BaudRate = 1 }; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var a = new SerialComConfiguration() { BaudRate = -1 }; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var a = new SerialComConfiguration() { BaudRate = 9599 }; });

            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.Odd, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.Even, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.Mark, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.Space, 6, StopBits.One);

            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 5, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 7, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 8, StopBits.One);
            Assert.Throws<ArgumentOutOfRangeException>(() => { var a = new SerialComConfiguration() { DataBits = 4 }; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var a = new SerialComConfiguration() { DataBits = 9 }; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var a = new SerialComConfiguration() { DataBits = 0 }; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var a = new SerialComConfiguration() { DataBits = -1 }; });

            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.None);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.Two);
            Helper.SCSerialize(0, "COM1", 9600, Parity.None, 6, StopBits.OnePointFive);
        }

        [Test]
        public void TestGenericConfigToString()
        {
            Helper.GCToString(GenericConfiguration.CommunicationType.NONE, 0);
            Helper.GCToString(GenericConfiguration.CommunicationType.NONE, 100);
            Helper.GCToString(GenericConfiguration.CommunicationType.NONE, -1);
            Helper.GCToString(GenericConfiguration.CommunicationType.SERIAL, 0);
            Helper.GCToString(GenericConfiguration.CommunicationType.SERIAL, 100);
            Helper.GCToString(GenericConfiguration.CommunicationType.SERIAL, -1);
        }

        [Test]
        public void TestSerialConfigToString()
        {
            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(100, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(-1, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(100, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(-1, "COM1", 9600, Parity.None, 6, StopBits.One);

            Helper.SCToString(0, "COM1", 110, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 300, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 600, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 1200, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 2400, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 4800, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 14400, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 19200, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 38400, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 57600, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 115200, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 128000, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 256000, Parity.None, 6, StopBits.One);

            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.Odd, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.Even, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.Mark, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.Space, 6, StopBits.One);

            Helper.SCToString(0, "COM1", 9600, Parity.None, 5, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 7, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 8, StopBits.One);

            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.None);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.One);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.Two);
            Helper.SCToString(0, "COM1", 9600, Parity.None, 6, StopBits.OnePointFive);
        }
    }
}