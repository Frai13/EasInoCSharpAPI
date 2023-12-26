using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using EasInoAPI;
using EasInoAPI.Configuration;

namespace EasInoCLI.Commands
{
    internal class SendData : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-s", "--send" };
        public IDictionary<string, string> Args => new Dictionary<string, string>()
        {
            { "Operation", "Operation to be performed" }
        };
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>()
        {
            { "Args", "Arguments of the operation splitted by space" }
        };

        public string Description => "Send data to EasIno board";

        public ICommand.ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            if (!Common.CheckArgsProvided(Args, ArgsProvided))
            {
                return;
            }

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

            string operation = ArgsProvided.ElementAt(0);
            IEnumerable<string> args = ArgsProvided.Skip(1);

            try
            {
                EasIno easino = Common.GetEasIno(config);

                easino.Send(new DataCom(operation, args));
                Console.WriteLine($"Operation \"{operation}\" sent successfully with args: {String.Join(" ", args)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }
        };
    }
}
