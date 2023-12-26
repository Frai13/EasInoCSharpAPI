using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasInoExamples
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Common.PrintHelp();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string cmd = args[0];
            IEnumerable<string> cmd_args = args.Skip(1);

            IEnumerable<Common.IExample> examples = Common.GetExamples().Where(c => c.Command.Contains(cmd));

            if (!examples.Any())
            {
                Console.WriteLine($"No match for example: {cmd}");
                return;
            }

            examples.First().Run(cmd_args);
        }
    }
}
