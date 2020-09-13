using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.DataSaving
{
    class IndexerEntry
    {
        public Level Level { get; set; }
        public SessionGroup Group { get; set; }

        public bool SameEntry(IndexerEntry entry)
        {
            return Group.IsSameGroup(entry.Group) && Level.IsSameLevel(entry.Level);
        }
    }
}
