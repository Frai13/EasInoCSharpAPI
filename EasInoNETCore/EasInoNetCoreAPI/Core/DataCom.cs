using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasInoAPI
{
    public class DataCom
    {
        internal const string Head = "EINO::";
        internal const string Tail = "::END";
        internal const char Separator = ';';

        /// <summary>
        /// Operation to be performed
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// Arguments of the operation
        /// </summary>
        public IEnumerable<string> Args { get; set; }

        /// <summary>
        /// DataCom constructor
        /// </summary>
        public DataCom()
        {
            Operation = "";
            Args = new List<string>();
        }
        /// <summary>
        /// DataCom constructor
        /// </summary>
        /// <param name="Operation"><see cref="Operation"/></param>
        /// <param name="Args"><see cref="Args"/></param>
        public DataCom(string Operation, IEnumerable<string> Args)
        {
            this.Operation = Operation;
            this.Args = Args;
        }

        internal DataCom(string line)
        {
            Operation = "";
            Args = new List<string>();

            int headIndex = line.IndexOf(Head);
            int tailIndex = line.IndexOf(Tail);

            if (headIndex == -1 || tailIndex == -1 || headIndex > tailIndex)
            {
                return;
            }

            string line_mod = line.Substring(headIndex + Head.Length, tailIndex - (headIndex + Head.Length));
            line_mod = Regex.Replace(line_mod, $"{Separator}$", "");

            IEnumerable<string> groups = line_mod.Split(Separator);

            Operation = groups.ElementAt(0);
            Args = groups.Skip(1);
        }

        public override string ToString()
        {
            return $"{Head}{Operation};{String.Join(Separator, Args)};{Tail}";
        }
    }
}
