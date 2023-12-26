using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using EasInoAPI;
using EasInoAPI.Configuration;

namespace EasInoCLI.Commands
{
    internal class SetDefaultConfigSerial : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-sdcs", "--setdefaultconfigserial" };
        public IDictionary<string, string> Args => new Dictionary<string, string>()
        {
            { "Port", "Name of the serial port" },
            { "BaudRate", "Baud rate to be used. Typical values are: 9600 and 115200" },
            { "Parity", "Parity to be used. Typical values are: Odd, None and Even" },
            { "DataBits", "DataBits to be used. Typical values are: 7 and 8" },
            { "StopBits", "StopBits to be used. Typical values are: None, One and Two" },
        };
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>()
        {
            { "Timeout", "Timeout of the response received" }
        };

        public string Description => "Set serial communication default configuration";

        public ICommand.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            if (!Common.CheckArgsProvided(Args, ArgsProvided))
            {
                return;
            }

            SerialComConfiguration config;
            try
            {
                config = new SerialComConfiguration(ArgsProvided);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }

            config.Serialize();

            Console.WriteLine($"{config}");
            Console.WriteLine($"Serial configuration set to default successfully");
        };
    }
}
