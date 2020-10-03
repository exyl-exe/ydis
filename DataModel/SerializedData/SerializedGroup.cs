using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel.SerializedData
{
    class SerializedGroup : IWdisSerialized
    {
        [JsonProperty(PropertyName = "GroupName")] public string GroupName { get; set; }

        public SerializedGroup() { }

        public SerializedGroup(SessionGroup g)
        {
            GroupName = g.GroupName;
        }
        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
