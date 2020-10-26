using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    static class GroupLoader
    {
        public static List<SessionGroup> GetAllGroups()//TODO don't deserialize sessions if not needed
        {
            var res = new List<SessionGroup>();
            var indexer = SessionSaver.DeserializeSessionManager();

            var directories = new HashSet<string>();
            foreach (var entry in indexer.Entries)
            {
                directories.Add(entry.Group.GetGroupDirectoryPath());
            }

            foreach (var dir in directories)
            {
                var sessionList = new List<Session>();
                var files = SafeDirectory.GetFiles(dir);
                foreach (var sessionFile in files)
                {
                    try
                    {
                        var session = SessionSaver.DeserializeSession(sessionFile);
                        sessionList.Add(session);
                    }
                    catch (JsonSerializationException)
                    {
                        //TODO
                       
                    }
                }
                if (sessionList.Count > 0)
                {
                    var group = new SessionGroup
                    {
                        GroupName = sessionList[0].Level.Name,//TODO name selection
                        GroupSessions = sessionList
                    };
                    res.Add(group);
                }
            }
            res.Sort((g1, g2) => string.Compare(g1.GroupName, g2.GroupName));
            return res;
        }
    }
}
