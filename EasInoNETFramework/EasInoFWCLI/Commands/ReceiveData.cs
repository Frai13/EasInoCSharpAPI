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
    internal class ReceiveData : ICommand
    {
        public IEnumerable<string> Command => new List<string>() { "-r", "--receive" };
        public IDictionary<string, string> Args => new Dictionary<string, string>();
        public IDictionary<string, string> OptionalArgs => new Dictionary<string, string>();

        public string Description => "Receive data from EasIno board";

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

                easino.DataReceived += Easino_DataReceived;

                Console.WriteLine($"Waiting data");

                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }
        };

        private void Easino_DataReceived(DataReceivedEventArgs args)
        {
            Console.WriteLine($"Received \"{args.Data.Operation}\" with args: {String.Join(" ", args.Data.Args)}");
        }
    }
}
