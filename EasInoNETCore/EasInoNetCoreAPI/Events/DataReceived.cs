using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasInoAPI
{
    /// <summary>
    /// Delegate of the method to implement when new Data is received
    /// </summary>
    /// <param name="args">Arguments of the even</param>
    public delegate void DataReceivedEventHandler(DataReceivedEventArgs args);

    /// <summary>
    /// Defines the data received object
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Data received
        /// </summary>
        public DataCom Data { get; }

        public DataReceivedEventArgs(DataCom data)
        {
            Data = data;
        }
    }
}
