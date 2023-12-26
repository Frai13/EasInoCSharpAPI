﻿using System;
using System.Collections.Generic;
using System.Linq;
using EasInoAPI;
using EasInoAPI.Configuration;
using static EasInoExamples.Common;
using static EasInoExamples.Common.IExample;

namespace EasInoExamples.Examples
{
    internal class LedOnOffClient : Common.IExample
    {
        public IEnumerable<string> Command => new List<string>() { "1", "LedOnOffClient" };

        /// <summary>
        ///  In this example, SerialCom is used to communicate with a Nano board that uses EasIno.
        ///  When board receives operation "LED13" it does different operations depending on first argument:
        ///  "0" -> turn off LED 13, "1" -> turn on LED 13.
        ///  Once this operation has been executed, board sends confirmation message:
        ///  operation -> "LED13", arg1 -> "0" if successfully turned off, "1" if successfully turned on, "-1" otherwise.
        ///  Since this example sends data and waits for response, it will be the master, and board will be the slave.
        /// </summary>
        /// <remarks>Visit "LedOnOffServer.ino" file in EasIno Arduino repository</remarks>
        public ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            SerialComConfiguration config = new SerialComConfiguration()
            {
                ComPort = ArgsProvided.ElementAt(0),
                BaudRate = 115200,
                Parity = System.IO.Ports.Parity.None,
                DataBits = 8,
                StopBits = System.IO.Ports.StopBits.One,
                Timeout = 2000
            };

            try
            {
                EasIno easino = new SerialCom(config);
                easino.Start();

                string dataOperation = "LED13";

                Console.WriteLine($"Turn on LED 13");
                easino.Send(new DataCom(dataOperation, new List<string>() { "1" }));
                Console.WriteLine($"Waiting confirmation");
                DataCom dataReceived = easino.Receive();
                Console.WriteLine($"Received \"{dataReceived.Operation}\" with args: {String.Join(" ", dataReceived.Args)}");

                System.Threading.Thread.Sleep(2000);

                Console.WriteLine($"{Environment.NewLine}Turn off LED 13");
                easino.Send(new DataCom(dataOperation, new List<string>() { "0" }));
                Console.WriteLine($"Waiting confirmation");
                dataReceived = easino.Receive();
                Console.WriteLine($"Received \"{dataReceived.Operation}\" with args: {String.Join(" ", dataReceived.Args)}");

                System.Threading.Thread.Sleep(2000);

                Console.WriteLine($"{Environment.NewLine}Send operation without known argument");
                easino.Send(new DataCom(dataOperation, new List<string>()));
                Console.WriteLine($"Waiting confirmation");
                dataReceived = easino.Receive();
                Console.WriteLine($"Received \"{dataReceived.Operation}\" with args: {String.Join(" ", dataReceived.Args)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }
        };
    }
}