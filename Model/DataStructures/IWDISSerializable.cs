using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// Serializable WDIS class.
    /// </summary>
    public abstract class IWDISSerializable
    {
        [JsonProperty(PropertyName = "Version")] const int Version = 1;//TODO config?
        /// <summary>
        /// Serializes the object in the JSON format.
        /// </summary>
        /// <returns>A string containing the json object.</returns>
        public abstract string Serialize();
        /// <summary>
        /// Initiliaze the object based on its json string.
        /// </summary>
        /// <param name="value">A string containing the json object.</param>
        public abstract void Deserialize(string value);
    }
}
