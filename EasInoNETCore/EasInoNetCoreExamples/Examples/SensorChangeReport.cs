using System.Data;
using EasInoAPI;
using EasInoAPI.Configuration;
using static EasInoExamples.Common.IExample;

namespace EasInoExamples.Examples
{
    internal class SensorChangeReport : Common.IExample
    {
        public IEnumerable<string> Command => new List<string>() { "3", "SensorChangeReport" };

        /// <summary>
        ///  In this example, SerialCom is used to communicate with a Nano board that uses EasIno.
        ///  Board has a pushbutton connected. When its value changes, it is reported.
        ///  Board has also a potentiometer. When its value changes more than a predefined value (<c>Threshold</c>), is is reported.
        ///  <c>Threshold</c> can be modified pressing key 'T' and sending it to board.
        ///  Push any key (except 'T') to stop execution.
        /// </summary>
        /// <remarks>Visit "SensorChangeReport.ino" file in EasIno Arduino repository</remarks>
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

                easino.DataReceived += Easino_DataReceived;

                Console.WriteLine($"Waiting data");

                while (Console.ReadKey(true).Key == ConsoleKey.T)
                {
                    Console.WriteLine($"Write new Threshold value (should be a number)");
                    string? line = Console.ReadLine();

                    if (line == null)
                    {
                        Console.WriteLine($"ERROR: bad Threshold format. Not changed.");
                        continue;
                    }

                    if (int.TryParse(line, out int Threshold))
                    {
                        easino.Send(new DataCom("THRESHOLD", new List<string>() { Threshold.ToString() }));
                        Console.WriteLine($"Change Threshold request sent successfully with value {Threshold}");

                        DataCom dataReceived = easino.Receive();
                        if (dataReceived.Operation == "THRESHOLD")
                        {
                            Console.WriteLine($"Received confirmation. Threshold changed to {dataReceived.Args.ElementAt(0)}");
                        }
                        else
                        {
                            Console.WriteLine($"Threshold change confirmation not received. Received: {dataReceived.Operation}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ERROR: {line} is not a number");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return;
            }
        };

        private static void Easino_DataReceived(DataReceivedEventArgs args)
        {
            if (args.Data.Operation == "BUTTON")
            {
                if (args.Data.Args.ElementAt(0) == "1")
                {
                    Console.WriteLine($"Button pressed.");
                }
                else if (args.Data.Args.ElementAt(0) == "0")
                {
                    Console.WriteLine($"Button released.");
                }
            }
            else if (args.Data.Operation == "POTENTIOMETER")
            {
                string value = args.Data.Args.ElementAt(0).ToString();
                string threshold = args.Data.Args.ElementAt(1).ToString();
                Console.WriteLine($"Potentiometer value changed to {value} (current threshold is {threshold})");
            }
            else
            {
                Console.WriteLine($"Unknown operation received: {args.Data.Operation}");
            }

        }
    }
}