using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using EasInoAPI;
using EasInoAPI.Configuration;

namespace EasInoAPI
{
    public class SerialCom : EasIno
    {
        private static SerialPort m_serialPort;

        private string buffer = "";
        private SerialComConfiguration Configuration = new SerialComConfiguration();

        /// <summary>
        /// Timeout of the response received
        /// </summary>
        public override int Timeout { get => Configuration.Timeout; set => Configuration.Timeout = value; }

        internal SerialCom() { }

        /// <summary>
        /// SerialCom constructor
        /// </summary>
        /// <param name="Config">Serial communication configuration</param>
        public SerialCom(SerialComConfiguration config)
        {
            m_serialPort = new SerialPort()
            {
                PortName = config.ComPort,
                BaudRate = config.BaudRate,
                Parity = config.Parity,
                DataBits = config.DataBits,
                StopBits = config.StopBits
            };

            Configuration = config;
        }

        /// <summary>
        /// SerialCom destructor
        /// </summary>
        ~SerialCom()
        {
            if (m_serialPort != null)
            {
                m_serialPort.DataReceived -= sp_DataReceived;
                m_serialPort.Close();
            }
        }

        /// <summary>
        /// Starts SerialCom communication
        /// </summary>
        public override void Start()
        {
            m_serialPort.DataReceived += sp_DataReceived;
            m_serialPort.Open();
        }

        /// <summary>
        /// Stops SerialCom communication
        /// </summary>
        public override void Stop()
        {
            if (m_serialPort != null)
            {
                m_serialPort.DataReceived -= sp_DataReceived;
                m_serialPort.Close();
            }
        }

        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string lineRec = m_serialPort.ReadExisting() ?? "";
            buffer += lineRec;
            if (buffer.Contains(DataCom.Tail))
            {
                string line = buffer;
                buffer = "";
                if (line != "")
                {
                    InvokeDataReceived(line);
                }
            }
        }

        protected override void DerivedSend(string data)
        {
            m_serialPort.Write(data);
        }
    }
}
