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
        private bool NotJoin = false;
        private DataCom dataCom = new DataCom() { Operation = "TEST" };
        private List<DataReceivedEventArgs> dataSubcribed = new List<DataReceivedEventArgs>();
        private Thread t;

        public override int Timeout { get => timeout; set => timeout = value; }
        private int timeout = 100;
        public override void Start() { }
        public override void Stop() { }
        protected override void DerivedSend(string data)
        {
            if (t != null && !NotJoin) t.Join();
            t = new Thread(() =>
            {
                Thread.Sleep(TimeSend);
                InvokeDataReceived(data.ToString());
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
            Assert.Throws<TimeoutException>(() => { SendAndReceive(dataCom); });
            t.Join();

            TimeSend = 250;
            NotJoin = true;
            d = TrySendAndReceive(dataCom, 3);
            t.Join();
            Assert.That(d == dataCom);
            TimeSend = 500;
            Assert.Throws<TimeoutException>(() => { TrySendAndReceive(dataCom, 3); });
            t.Join();
            NotJoin = false;
        }

        [Test]
        public void TestSubcribe()
        {
            dataSubcribed = new List<DataReceivedEventArgs>();
            DataReceived += TestsEasIno_DataReceived;

            TimeSend = 50;
            DataCom d = SendAndReceive(dataCom);
            Assert.That(d == dataCom);
            Assert.That(dataSubcribed.Count() == 0);

            Send(dataCom);
            t.Join();
            Assert.That(dataSubcribed.Count() == 1);
            Assert.That(dataSubcribed.Last().Data == dataCom);

            dataSubcribed = new List<DataReceivedEventArgs>();
            Send(dataCom);
            Send(dataCom);
            Send(dataCom);
            t.Join();
            Assert.That(dataSubcribed.Count() == 3);
        }

        private void TestsEasIno_DataReceived(DataReceivedEventArgs args)
        {
            dataSubcribed.Add(args);
        }
    }
}
