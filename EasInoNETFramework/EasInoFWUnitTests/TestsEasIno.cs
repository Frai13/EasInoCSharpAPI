using EasInoAPI;
using EasInoAPI.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasInoFWUnitTests
{
    public class TestsEasIno : EasIno
    {
        private int TimeSend = 0;
        private int MsgSent = 0;
        private DataCom dataCom = new DataCom() { Operation = "TEST" };
        private List<DataReceivedEventArgs> dataSubcribed = new List<DataReceivedEventArgs>();

        public override int Timeout { get => timeout; set => timeout = value; }
        private int timeout = 100;
        public override void Start() { }
        public override void Stop() { }
        protected override void DerivedSend(string data)
        {
            Thread t = new Thread(() =>
            {
                Thread.Sleep(TimeSend);
                InvokeDataReceived(data.ToString());
                MsgSent++;
            });
            t.Start();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestVersion()
        {
            Assert.That(EasIno.GetVersion() == "v1.0.1");
        }

        [Test]
        public void TestReceive()
        {
            Assert.Throws<TimeoutException>(() => { Receive(); });
            TimeSend = 50;
            DataCom d = SendAndReceive(dataCom);
            Assert.That(d == dataCom);
            d = SendAndReceive(new DataCom());
            Assert.That(d == new DataCom());
            TimeSend = 150;
            MsgSent = 0;
            Assert.Throws<TimeoutException>(() => { SendAndReceive(dataCom); });
            while (MsgSent != 1) Thread.Sleep(10);

            TimeSend = 250;
            MsgSent = 0;
            d = TrySendAndReceive(dataCom, 3);
            Assert.That(d == dataCom);
            while (MsgSent != 3) Thread.Sleep(10);
            TimeSend = 500;
            MsgSent = 0;
            Assert.Throws<TimeoutException>(() => { TrySendAndReceive(dataCom, 3); });
            while (MsgSent != 3) Thread.Sleep(10);
        }

        [Test]
        public void TestSubcribe()
        {
            dataSubcribed = new List<DataReceivedEventArgs>();
            DataReceived += TestsEasIno_DataReceived;

            TimeSend = 50;
            MsgSent = 0;
            DataCom d = SendAndReceive(dataCom);
            Assert.That(d == dataCom);
            while (MsgSent != 1) Thread.Sleep(10);
            Assert.That(dataSubcribed.Count() == 0);

            MsgSent = 0;
            Send(dataCom);
            while (MsgSent != 1) Thread.Sleep(10);
            Assert.That(dataSubcribed.Count() == 1);
            Assert.That(dataSubcribed.Last().Data == dataCom);

            dataSubcribed = new List<DataReceivedEventArgs>();
            MsgSent = 0;
            Send(dataCom);
            Send(dataCom);
            Send(dataCom);
            Thread.Sleep(150);
            while (MsgSent != 3) Thread.Sleep(10);
            Assert.That(dataSubcribed.Count() == 3);
        }

        private void TestsEasIno_DataReceived(DataReceivedEventArgs args)
        {
            dataSubcribed.Add(args);
        }
    }
}
