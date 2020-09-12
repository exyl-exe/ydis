using System;
using System.Collections.Generic;
using System.IO;
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
        public string SessionName { get; set; }

        public string GetSessionPath()
        {
            return Path.Combine(Group.GetGroupDirectoryPath(), SessionName);
        }
    }
}
