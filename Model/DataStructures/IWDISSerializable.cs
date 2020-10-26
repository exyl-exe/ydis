using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.DataStructures
{
    public abstract class IWDISSerializable
    {
        [JsonProperty(PropertyName = "Version")] const int Version = 1;
        public abstract string Serialize();
        public abstract void Deserialize(string value);
    }
}
