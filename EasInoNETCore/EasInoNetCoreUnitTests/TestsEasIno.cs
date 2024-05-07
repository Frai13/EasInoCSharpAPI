using EasInoAPI;
using EasInoAPI.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.IO.Ports;
using System.Threading;

namespace EasInoNetCoreUnitTests
{
    public class TestsEasIno : EasIno
    {
        private int TimeSend = 0;
        private bool MsgSent = true;
        private DataCom dataCom = new DataCom() { Operation = "TEST" };
        private List<DataReceivedEventArgs> dataSubcribed = new List<DataReceivedEventArgs>();

        public override int Timeout { get => timeout; set => timeout = value; }
        private int timeout = 100;
        public override void Start() { }
        public override void Stop() { }
        protected override void DerivedSend(string data)
        {
            MsgSent = false;
            Thread t = new Thread(() =>
            {
                Thread.Sleep(TimeSend);
                InvokeDataReceived(data.ToString());
                MsgSent = true;
            });
            t.Start();
        }

        [SetUp]
        public void Setup()
        {
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
            while (!MsgSent) Thread.Sleep(10);
        }

        [Test]
        public void TestSubcribe()
        {
            dataSubcribed = new List<DataReceivedEventArgs>();
            DataReceived += TestsEasIno_DataReceived;

            TimeSend = 50;
            DataCom d = SendAndReceive(dataCom);
            Assert.That(d == dataCom);
            Thread.Sleep(150);
            Assert.That(dataSubcribed.Count() == 0);

            Send(dataCom);
            Thread.Sleep(150);
            Assert.That(dataSubcribed.Count() == 1);
            Assert.That(dataSubcribed.Last().Data == dataCom);

            dataSubcribed = new List<DataReceivedEventArgs>();
            Send(dataCom);
            Send(dataCom);
            Send(dataCom);
            Thread.Sleep(150);
            Assert.That(dataSubcribed.Count() == 3);
            while (!MsgSent) Thread.Sleep(10);
        }

        private void TestsEasIno_DataReceived(DataReceivedEventArgs args)
        {
            dataSubcribed.Add(args);
        }
    }
}