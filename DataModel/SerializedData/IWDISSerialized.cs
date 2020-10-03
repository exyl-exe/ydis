using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel
{
    public abstract class IWdisSerialized
    {
        [JsonProperty(PropertyName = "Version")] const int Version = 1;
        public abstract string Serialize();
    }
}
