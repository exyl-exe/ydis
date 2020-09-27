using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;

namespace Whydoisuck.DataSaving
{
    class IndexerEntry
    {
        [JsonProperty(PropertyName = "Level")] public Level Level { get; set; }
        [JsonProperty(PropertyName = "Group")] public SessionGroup Group { get; set; }

        public bool SameEntry(IndexerEntry entry)
        {
            return Group.IsSameGroup(entry.Group) && Level.IsSameLevel(entry.Level);
        }
    }
}
