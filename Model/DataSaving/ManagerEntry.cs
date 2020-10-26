using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.DataSaving
{
    class ManagerEntry
    {
        [JsonProperty(PropertyName = "Level")] public Level Level { get; set; }
        [JsonProperty(PropertyName = "Group")] public SessionGroup Group { get; set; }

        public ManagerEntry() { }

        public bool SameEntry(ManagerEntry entry)
        {
            return Group.IsSameGroup(entry.Group) && Level.IsSameLevel(entry.Level);
        }
    }
}
