using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

namespace EasInoAPI.Configuration
{
    public class SerialComConfiguration : GenericConfiguration
    {
        /// <summary>
        /// System serial port name
        /// </summary>
        public string ComPort { get; set; } = "";

        /// <summary>
        /// Baud rate to be used in the communication
        /// </summary>
        /// <value>110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000 or 256000</value>
        public int BaudRate { get { return _BaudRate; }
            set
            {
                if (value != 110 && value != 300 && value != 600 && value != 1200 && value != 2400 && value != 4800 && value != 9600 && value != 14400
                    && value != 19200 && value != 38400 && value != 57600 && value != 115200 && value != 128000 && value != 256000)
                {
                    throw new ArgumentOutOfRangeException($"BaudRate invalid value {value}");
                }
                _BaudRate = value;
            }
        }
        private int _BaudRate = 9600;

        /// <summary>
        /// Parity param. See <see cref="System.IO.Ports.Parity"/>
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;

        /// <summary>
        /// DataBits param
        /// </summary>
        /// <value>5, 6, 7 or 8</value>
        public int DataBits
        {
            get { return _DataBits; }
            set
            {
                if (value < 5 || value > 8)
                {
                    throw new ArgumentOutOfRangeException($"DataBits invalid value {value}");
                }
                _DataBits = value;
            }
        }
        private int _DataBits = 8;

        /// <summary>
        /// StopBits param. See <see cref="System.IO.Ports.StopBits"/>
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;

        /// <summary>
        /// SerialComConfiguration default constructor
        /// </summary>
        public SerialComConfiguration()
        {
            this.ComType = CommunicationType.SERIAL;
        }

        /// <summary>
        /// SerialComConfiguration constructor
        /// </summary>
        /// <param name="ComPort"><see cref="ComPort"/></param>
        public SerialComConfiguration(string ComPort)
        {
            this.ComType = CommunicationType.SERIAL;
            this.ComPort = ComPort;
        }

        /// <summary>
        /// SerialComConfiguration constructor
        /// </summary>
        /// <param name="ComPort"><see cref="ComPort"/></param>
        /// <param name="BaudRate"><see cref="BaudRate"/></param>
        public SerialComConfiguration(string ComPort, int BaudRate)
        {
            this.ComType = CommunicationType.SERIAL;
            this.ComPort = ComPort;
            this.BaudRate = BaudRate;
        }

        /// <summary>
        /// SerialComConfiguration constructor
        /// </summary>
        /// <param name="ComPort"><see cref="ComPort"/></param>
        /// <param name="BaudRate"><see cref="BaudRate"/></param>
        /// <param name="Parity"><see cref="Parity"/></param>
        /// <param name="DataBits"><see cref="DataBits"/></param>
        /// <param name="StopBits"><see cref="StopBits"/></param>
        public SerialComConfiguration(string ComPort, int BaudRate, Parity Parity, int DataBits, StopBits StopBits)
        {
            this.ComType = CommunicationType.SERIAL;
            this.ComPort = ComPort;
            this.BaudRate = BaudRate;
            this.Parity = Parity;
            this.DataBits = DataBits;
            this.StopBits = StopBits;
        }

        /// <summary>
        /// SerialComConfiguration constructor
        /// </summary>
        /// <param name="ComPort"><see cref="ComPort"/></param>
        /// <param name="BaudRate"><see cref="BaudRate"/></param>
        /// <param name="Parity"><see cref="Parity"/></param>
        /// <param name="DataBits"><see cref="DataBits"/></param>
        /// <param name="StopBits"><see cref="StopBits"/></param>
        /// <param name="Timeout"><see cref="Timeout"/></param>
        public SerialComConfiguration(string ComPort, int BaudRate, Parity Parity, int DataBits, StopBits StopBits, int Timeout)
        {
            this.ComType = CommunicationType.SERIAL;
            this.ComPort = ComPort;
            this.BaudRate = BaudRate;
            this.Parity = Parity;
            this.DataBits = DataBits;
            this.StopBits = StopBits;
            this.Timeout = Timeout;
        }

        public SerialComConfiguration(IEnumerable<string> args)
        {
            this.ComType = CommunicationType.SERIAL;

            this.ComPort = args.ElementAt(0);

            int BaudRate;
            if (!Int32.TryParse(args.ElementAt(1), out BaudRate))
            {
                throw new ArgumentException($"Could not parse {args.ElementAt(1)}");
            }
            this.BaudRate = BaudRate;

            Parity Parity;
            if (!Enum.TryParse(args.ElementAt(2), out Parity))
            {
                throw new ArgumentException($"Could not parse {args.ElementAt(2)}");
            }
            this.Parity = Parity;

            int DataBits;
            if (!Int32.TryParse(args.ElementAt(3), out DataBits))
            {
                throw new ArgumentException($"Could not parse {args.ElementAt(3)}");
            }
            this.DataBits = DataBits;

            StopBits StopBits;
            if (!Enum.TryParse(args.ElementAt(4), out StopBits))
            {
                throw new ArgumentException($"Could not parse {args.ElementAt(4)}");
            }
            this.StopBits = StopBits;

            if (args.Count() < 6)
            {
                return;
            }

            int Timeout;
            if (!Int32.TryParse(args.ElementAt(5), out Timeout))
            {
                throw new ArgumentException($"Could not parse {args.ElementAt(5)}");
            }
            this.Timeout = Timeout;
        }

        internal override void Serialize()
        {
            string jsonString = JsonConvert.SerializeObject(this);

            File.WriteAllText("CommunicationConfiguration.json", jsonString);
        }
    }
}
