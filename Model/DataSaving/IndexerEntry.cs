using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.DataModel.SerializedData.SaveManager;

namespace Whydoisuck.DataSaving
{
    class IndexerEntry
    {
        public Level Level { get; set; }
        public SessionGroup Group { get; set; }

        public IndexerEntry() { }
        public IndexerEntry(SerializedManagerEntry e)
        {
            Level = new Level(e.Level);
            Group = new SessionGroup(e.Group);
        }

        public bool SameEntry(IndexerEntry entry)
        {
            return Group.IsSameGroup(entry.Group) && Level.IsSameLevel(entry.Level);
        }
    }
}
