using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.DataModel.SerializedData.SaveManager
{
    class SerializedManagerEntry
    {
        [JsonProperty(PropertyName = "Level")] public SerializedLevel Level { get; set; }
        [JsonProperty(PropertyName = "Group")] public SerializedGroup Group { get; set; }

        public SerializedManagerEntry() { }

        public SerializedManagerEntry(IndexerEntry e)
        {
            Level = new SerializedLevel(e.Level);
            Group = new SerializedGroup(e.Group);
        }
    }
}
