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
    internal class GetBoardVersion : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-gbv", "--getboardversion" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Get EasIno board version";

        public Common.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
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
                string version = easino.GetBoardVersion();

                Console.WriteLine($"EasIno board Version is: {version}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }
        };
    }
}
