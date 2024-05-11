using EasInoAPI.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("EasInoCLI"), InternalsVisibleTo("EasInoFWUnitTests")]
namespace EasInoAPI
{
    public abstract class EasIno
    {
        private enum ReceiveState { IDLE, WAITING, RECEIVED }

        private ReceiveState receiveState = ReceiveState.IDLE;
        private DataCom data = new DataCom();

        /// <summary>
        /// Timeout of the response received
        /// </summary>
        public abstract int Timeout { get; set; }

        protected abstract void DerivedSend(string data);

        /// <summary>
        /// Event raised when new data is received
        /// </summary>
        public event DataReceivedEventHandler DataReceived
        {
            add => _DataReceived += value;
            remove => _DataReceived -= value;
        }
        protected event DataReceivedEventHandler _DataReceived;

        /// <summary>
        /// Starts active communication
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops active communication
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Sends data through the active communication
        /// </summary>
        /// <param name="data">Data to be sent</param>
        public void Send(DataCom data)
        {
            string line = data.ToString();
            DerivedSend(line != null ? line : "");
        }

        /// <summary>
        /// Receives data through the active communication until a maximum of <see cref="Timeout"/> milliseconds
        /// </summary>
        /// <returns>Data received</returns>
        public DataCom Receive()
        {
            return Receive(Timeout);
        }

        /// <summary>
        /// Receives data through the active communication until a maximum of <paramref name="timeout"/> milliseconds
        /// </summary>
        /// <param name="timeout">Milliseconds until timeout</param>
        /// <returns>Data received</returns>
        public DataCom Receive(int timeout)
        {
            receiveState = ReceiveState.WAITING;
            DateTime dateSent = DateTime.Now;
            while (receiveState != ReceiveState.RECEIVED)
            {
                if ((DateTime.Now - dateSent).TotalMilliseconds > timeout)
                {
                    receiveState = ReceiveState.IDLE;
                    throw new TimeoutException($"Timeout ({timeout}ms)");
                }
                Thread.Sleep(10);
            }

            receiveState = ReceiveState.IDLE;
            return data;
        }

        /// <summary>
        /// Sends and receives data through the active communication until a maximum of <see cref="Timeout"/> milliseconds
        /// </summary>
        /// <param name="data">Data to be sent</param>
        /// <returns>Data received</returns>
        public DataCom SendAndReceive(DataCom data)
        {
            return SendAndReceive(data, Timeout);
        }

        /// <summary>
        /// Sends and receives data through the active communication until a maximum of <paramref name="timeout"/> milliseconds
        /// </summary>
        /// <param name="data">Data to be sent</param>
        /// <param name="timeout">Milliseconds until timeout</param>
        /// <returns>Data received</returns>
        public DataCom SendAndReceive(DataCom data, int timeout)
        {
            receiveState = ReceiveState.WAITING;
            Send(data);
            return Receive(timeout);
        }

        /// <summary>
        /// Tries to send and receive data a maximum number of <paramref name="retries"/> through the active communication until a maximum of <see cref="Timeout"/> milliseconds
        /// </summary>
        /// <param name="data">Data to be sent</param>
        /// <param name="retries">Maximum number of retries to receive</param>
        /// <returns>Data received</returns>
        public DataCom TrySendAndReceive(DataCom data, int retries)
        {
            return TrySendAndReceive(data, retries, Timeout);
        }

        /// <summary>
        /// Tries to send and receive data a maximum number of <paramref name="retries"/> through the active communication until a maximum of <paramref name="timeout"/> milliseconds
        /// </summary>
        /// <param name="data">Data to be sent</param>
        /// <param name="retries">Maximum number of retries to receive</param>
        /// <param name="timeout">Milliseconds until timeout</param>
        /// <returns>Data received</returns>
        public DataCom TrySendAndReceive(DataCom data, int retries, int timeout)
        {
            int i = 0;
            while (i < retries)
            {
                try
                {
                    receiveState = ReceiveState.WAITING;
                    Send(data);
                    return Receive(timeout);
                }
                catch (Exception)
                {
                    i++;
                    if (i >= retries)
                    {
                        throw;
                    }
                }
            }
            return new DataCom();
        }

        protected void InvokeDataReceived(string line)
        {
            if (receiveState == ReceiveState.WAITING)
            {
                data = new DataCom(line);
                receiveState = ReceiveState.RECEIVED;
            }
            else
            {
                _DataReceived?.Invoke(new DataReceivedEventArgs(new DataCom(line)));
            }
        }

        /// <summary>
        /// Retrieves the API version
        /// </summary>
        /// <returns>API version</returns>
        public static string GetVersion()
        {
            return Common.Version;
        }

        /// <summary>
        /// Request the EasIno board version through the active communication until a maximum of <see cref="Timeout"/> milliseconds
        /// </summary>
        /// <returns>EasIno board version</returns>
        public string GetBoardVersion()
        {
            return GetBoardVersion(Timeout);
        }

        /// <summary>
        /// Request the EasIno board version through the active communication until a maximum of <paramref name="timeout"/> milliseconds
        /// </summary>
        /// <returns>EasIno board version</returns>
        public string GetBoardVersion(int timeout)
        {
            DataCom data = new DataCom() { Operation = "VERSION" };
            Send(data);

            DataCom dataOut = Receive(timeout);

            if (dataOut.Operation == "VERSION")
            {
                return dataOut.Args.ElementAt(0);
            }

            throw new Exception($"Incorrect response received");
        }

        /// <summary>
        /// Makes ping through the active communication until a maximum of <see cref="Timeout"/> milliseconds
        /// </summary>
        /// <returns>True if EasIno board answered to ping, False otherwise</returns>
        public bool Ping()
        {
            return Ping(Timeout);
        }

        /// <summary>
        /// Makes ping through the active communication until a maximum of <paramref name="timeout"/> milliseconds
        /// </summary>
        /// <returns>True if EasIno board answered to ping, False otherwise</returns>
        public bool Ping(int timeout)
        {
            DataCom data = new DataCom() { Operation = "PING" };
            Send(data);

            DataCom dataOut = Receive(timeout);

            if (dataOut.Operation == "PING")
            {
                return true;
            }

            return false;
        }
    }
}
