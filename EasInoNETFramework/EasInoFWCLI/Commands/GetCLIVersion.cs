using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasInoAPI;

namespace EasInoCLI.Commands
{
    internal class GetCLIVersion : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-gcv", "--getcliversion" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Get EasIno CLI version";

        public Common.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            string version = Common.Version;

            Console.WriteLine($"EasIno CLI Version is: {version}");
        };
    }
}
