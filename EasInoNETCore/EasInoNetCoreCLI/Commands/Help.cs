using EasInoAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasInoCLI.Commands
{
    internal class Help : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-h", "--help" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Show EasIno help";

        public ICommand.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            string help = "";
            help += $"This is EasIno CLI program to help user executing commands in a fast way.{Environment.NewLine}" +
                    $"  API Version is {EasIno.GetVersion()}{Environment.NewLine}" +
                    $"  CLI Version is {Common.Version}{Environment.NewLine}" +
                    Environment.NewLine +
                    $"Command list:{Environment.NewLine}";

            foreach (ICommand c in Common.GetCommands().OrderBy(a => a.Command.First()))
            {
                help += $"  {String.Join(" / ", c.Command)}: {c.Description}{Environment.NewLine}";
                
                if (c.Args.Any())
                {
                    help += $"    {String.Join(" ", c.Args.Select(a => $"[{a.Key}]"))} {String.Join(" ", c.OptionalArgs.Select(a => $"<{a.Key}>"))}{Environment.NewLine}";
                    c.Args.ToList().ForEach(a => help += $"    - {a.Key}: {a.Value}{Environment.NewLine}");
                }
                if (c.OptionalArgs.Any())
                {
                    c.OptionalArgs.ToList().ForEach(a => help += $"    - {a.Key} (optional): {a.Value}{Environment.NewLine}");
                }
            }

            Console.WriteLine(help);
        };
    }
}
