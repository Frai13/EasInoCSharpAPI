using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasInoAPI;

namespace EasInoCLI.Commands
{
    internal class GetApiVersion : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-gav", "--getapiversion" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Get EasIno API version";

        public ICommand.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            string version = EasIno.GetVersion();

            Console.WriteLine($"EasIno API Version is: {version}");
        };
    }
}
