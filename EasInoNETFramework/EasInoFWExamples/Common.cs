using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasInoExamples
{
    internal static class Common
    {
        public delegate void ActionArgs(IEnumerable<string> ArgsProvided);

        internal interface IExample
        {
            IEnumerable<string> Command { get; }
            ActionArgs Run { get; }
        }

        internal static IEnumerable<IExample> GetExamples()
        {
            var type = typeof(IExample);
            IEnumerable<IExample> commandsNull = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type)
                .Select(s => (IExample)Activator.CreateInstance(s));

            IList<IExample> commands = new List<IExample>();
            foreach (var command in commandsNull)
            {
                if (command != null) commands.Add(command);
            }
            return commands;
        }

        internal static void PrintHelp()
        {
            string help = "";
            help += $"This is EasIno Examples program.{Environment.NewLine}" +
                    $"Examples list:{Environment.NewLine}";

            foreach (IExample e in Common.GetExamples().OrderBy(a => a.Command.First()))
            {
                help += $"  {String.Join(" / ", e.Command)}{Environment.NewLine}";
            }

            Console.WriteLine(help);
        }
    }
}
