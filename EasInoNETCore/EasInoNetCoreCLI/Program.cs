using EasInoCLI.Commands;

namespace EasInoCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Help help = new Help();
                help.Run(new List<string>());
                return;
            }

            string cmd = args[0];
            IEnumerable<string> cmd_args = args.Skip(1);
            
            IEnumerable<ICommand> command = Common.GetCommands().Where(c => c.Command.Contains(cmd));

            if (!command.Any())
            {
                Console.WriteLine($"No match for command: {cmd}");
                return;
            }

            command.First().Run(cmd_args);
        }
    }
}