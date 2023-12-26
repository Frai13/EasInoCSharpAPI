using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

[assembly: InternalsVisibleTo("EasInoCLI"), InternalsVisibleTo("EasInoNetCoreUnitTests")]
namespace EasInoAPI.Configuration
{
    public class GenericConfiguration
    {
        /// <summary>
        /// Enum of the communication type used
        /// </summary>
        public enum CommunicationType { NONE, SERIAL };

        /// <summary>
        /// Communication type used
        /// </summary>
        public CommunicationType ComType { get; set; } = CommunicationType.NONE;

        /// <summary>
        /// Timeout of the response received
        /// </summary>
        public int Timeout { get; set; } = 2000;

        public GenericConfiguration() { }

        /// <summary>
        /// GenericConfiguration constructor
        /// </summary>
        /// <param name="ComType"><see cref="ComType"/></param>
        /// <param name="Timeout"><see cref="Timeout"/></param>
        public GenericConfiguration(CommunicationType ComType, int Timeout)
        {
            this.ComType = ComType;
            this.Timeout = Timeout;
        }

        internal virtual void Serialize()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(this, options);

            File.WriteAllText("CommunicationConfiguration.json", jsonString);
        }

        internal GenericConfiguration Deserialize()
        {
            string jsonString = File.ReadAllText("CommunicationConfiguration.json");
            GenericConfiguration? conf = JsonSerializer.Deserialize<GenericConfiguration>(jsonString);

            if (conf == null)
            {
                return new GenericConfiguration();
            }

            if (conf.ComType == CommunicationType.SERIAL)
            {
                return JsonSerializer.Deserialize<SerialComConfiguration>(jsonString)!;
            }
            else
            {
                return conf;
            }
        }

        /// <summary>
        /// ToString override method
        /// </summary>
        /// <returns>Params formatted</returns>
        public override string ToString()
        {
            string outStr = "";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                if (descriptor.PropertyType != typeof(CommunicationType))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(this)!;
                    outStr += $"    - {name} = {value}{Environment.NewLine}";
                }
            }

            return outStr;
        }
    }
}
