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
    internal class GetDefaultConfigSerial : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-gdc", "--getdefaultconfig" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Get communication default configuration";

        public ICommand.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            GenericConfiguration config = new GenericConfiguration();
            try
            {
                config = config.Deserialize();
            }
            catch (Exception)
            {
                Console.WriteLine($"ERROR: no default configuration saved");
                return;
            }

            Console.WriteLine($"{config}");
            Console.WriteLine($"Default configuration read successfully");
        };
    }
}
