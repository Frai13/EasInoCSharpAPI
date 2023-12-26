using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasInoAPI;
using EasInoAPI.Configuration;

namespace EasInoCLI.Commands
{
    internal class Ping : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-p", "--ping" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Do ping to an EasIno board";

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
            
            try
            {
                EasIno easino = Common.GetEasIno(config);
                bool ok = easino.Ping();
                if (ok)
                {
                    Console.WriteLine($"EasIno ping received successfully");
                }
                else
                {
                    Console.WriteLine($"ERROR: EasIno ping not received");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }
        };
    }
}
