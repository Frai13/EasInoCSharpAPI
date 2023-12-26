using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasInoAPI
{
    internal static class Common
    {
        internal static string Version
        {
            get { return "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(3); }
        }
    }
}
