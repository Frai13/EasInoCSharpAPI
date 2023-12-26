using EasInoAPI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasInoCLI
{
    internal interface ICommand
    {
        public IEnumerable<string> Command { get; }
        public IDictionary<string, string> Args { get; }
        public IDictionary<string, string> OptionalArgs { get; }

        public string Description { get; }

        public delegate void ActionArgs(IEnumerable<string> ArgsProvided);
        public ActionArgs Run { get; }
    }
}
