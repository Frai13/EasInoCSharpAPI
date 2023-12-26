using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasInoAPI;
using EasInoAPI.Configuration;

namespace EasInoFWUnitTests
{
    public static class Helper
    {
        public static void GCSerialize(GenericConfiguration.CommunicationType comType, int timeout)
        {
            GenericConfiguration gc = new GenericConfiguration(comType, timeout);
            gc.Serialize();
            gc = gc.Deserialize();
            Assert.That(gc.ComType == comType);
            Assert.That(gc.Timeout == timeout);
        }

        public static void SCSerialize(int timeout, string comPort, int baudRate, Parity parity,
            int dataBits, StopBits stopBits)
        {
            SerialComConfiguration sc = new SerialComConfiguration(comPort, baudRate, parity, dataBits, stopBits, timeout);
            sc.Serialize();
            GenericConfiguration gc;
            gc = (SerialComConfiguration)((GenericConfiguration)sc).Deserialize();
            Assert.That(sc.ComType == GenericConfiguration.CommunicationType.SERIAL);
            Assert.That(sc.ComPort == comPort);
            Assert.That(sc.Timeout == timeout);
            Assert.That(sc.BaudRate == baudRate);
            Assert.That(sc.Parity == parity);
            Assert.That(sc.DataBits == dataBits);
            Assert.That(sc.StopBits == stopBits);
        }

        public static void GCToString(GenericConfiguration.CommunicationType comType, int timeout)
        {
            GenericConfiguration gc = new GenericConfiguration(comType, timeout);
            string str = gc.ToString();
            Assert.That(str == $"    - Timeout = {timeout}{Environment.NewLine}");
        }

        public static void SCToString(int timeout, string comPort, int baudRate, Parity parity,
            int dataBits, StopBits stopBits)
        {
            SerialComConfiguration sc = new SerialComConfiguration(comPort, baudRate, parity, dataBits, stopBits, timeout);
            string str = sc.ToString();
            Assert.That(str ==
                $"    - ComPort = {comPort}{Environment.NewLine}" +
                $"    - BaudRate = {baudRate}{Environment.NewLine}" +
                $"    - Parity = {parity}{Environment.NewLine}" +
                $"    - DataBits = {dataBits}{Environment.NewLine}" +
                $"    - StopBits = {stopBits}{Environment.NewLine}" +
                $"    - Timeout = {timeout}{Environment.NewLine}");
        }

        public static void SCEnumerableConstructor(int timeout, string comPort, int baudRate, Parity parity,
            int dataBits, StopBits stopBits)
        {
            SerialComConfiguration sc = new SerialComConfiguration(new List<string>() { comPort.ToString(), baudRate.ToString(),
                parity.ToString(), dataBits.ToString(), stopBits.ToString(), timeout.ToString(), });
            sc.Serialize();
            GenericConfiguration gc;
            gc = (SerialComConfiguration)((GenericConfiguration)sc).Deserialize();
            Assert.That(sc.ComType == GenericConfiguration.CommunicationType.SERIAL);
            Assert.That(sc.ComPort == comPort);
            Assert.That(sc.Timeout == timeout);
            Assert.That(sc.BaudRate == baudRate);
            Assert.That(sc.Parity == parity);
            Assert.That(sc.DataBits == dataBits);
            Assert.That(sc.StopBits == stopBits);
        }

        public static void DataComConstructor(string line, List<string> results)
        {
            DataCom dataCom = new DataCom(line);

            for (int i = results.Count; i < dataCom.Args.Count() + 1; i++)
            {
                results.Add("");
            }

            Assert.That(dataCom.Operation == results[0]);

            for (int i = 0; i < dataCom.Args.Count(); i++)
            {
                Assert.That(dataCom.Args.ElementAt(i) == results[i + 1]);
            }
        }
    }
}
