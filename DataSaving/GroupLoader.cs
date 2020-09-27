using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.UI;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    static class GroupLoader
    {
        public static List<GroupDisplayer> GetAllGroups()
        {
            var res = new List<GroupDisplayer>();
            var indexer = SessionSaver.LoadSessionManager();

            var directories = new HashSet<string>();
            foreach(var entry in indexer.Entries)
            {
                directories.Add(entry.Group.GetGroupDirectoryPath());
            }

            foreach(var dir in directories)
            {
                var sessionList = new List<Session>();
                var files = SafeDirectory.GetFiles(dir);
                foreach (var sessionFile in files)
                {
                    try
                    {
                        var jsonData = File.ReadAllText(sessionFile);
                        var session = JsonConvert.DeserializeObject<Session>(jsonData);
                        sessionList.Add(session);
                    } catch (JsonSerializationException)
                    {
                        DebugLogger.AddLog("Couldn't deserialize file : "+SafePath.GetFileName(sessionFile));
                    }
                }
                if (sessionList.Count > 0)
                {
                    var group = new GroupDisplayer
                    {
                        GroupName = sessionList[0].Level.Name,
                        GroupSessions = sessionList
                    };
                    res.Add(group);
                }
            }
            res.Sort( (g1, g2) => string.Compare(g1.GroupName, g2.GroupName) );
            return res;
        }
    }
}
