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
        IEnumerable<string> Command { get; }
        IDictionary<string, string> Args { get; }
        IDictionary<string, string> OptionalArgs { get; }

        string Description { get; }
        Common.ActionArgs Run { get; }
    }
}
