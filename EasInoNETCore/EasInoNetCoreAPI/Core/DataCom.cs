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
            line_mod = !line_mod.EndsWith($"{Separator}") ? line_mod : line_mod.Remove(line_mod.Length - 1);

            IEnumerable<string> groups = line_mod.Split(Separator);

            Operation = groups.ElementAt(0);
            Args = groups.Skip(1).ToList();
        }

        public override string ToString()
        {
            if (Args.Count() > 0)
            {
                return $"{Head}{Operation};{String.Join(Separator.ToString(), Args)};{Tail}";
            }
            else
            {
                return $"{Head}{Operation};{Tail}";
            }
        }

        public static bool operator ==(DataCom d1, DataCom d2)
        {
            if ((object)d1 == null)
                return (object)d2 == null;

            return d1.Equals(d2);
        }

        public static bool operator !=(DataCom d1, DataCom d2)
        {
            return !(d1 == d2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var d2 = (DataCom)obj;
            bool argsEqual = Args.Count() == d2.Args.Count() && (!Args.Except(d2.Args).Any() || !d2.Args.Except(Args).Any());
            return (Operation == d2.Operation && argsEqual);
        }

        public override int GetHashCode()
        {
            return (Operation, Args).GetHashCode();
        }
    }
}
