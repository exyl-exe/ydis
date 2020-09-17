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
            var groupList = new List<GroupDisplayer>();
            var directories = SafeDirectory.GetDirectories(SessionSaver.SAVE_DIR);

            var dirLog = "Directories:";//TODO remove
            foreach(var d in directories)
            {
                dirLog += "\n" + d;
            }
            TempLogger.AddLog(dirLog);

            foreach(var dir in directories)
            {
                var sessionList = new List<Session>();
                var files = SafeDirectory.GetFiles(dir);

                var filesLog = $"Files in {dir}:";//TODO remove
                foreach (var f in files)
                {
                    filesLog += "\n" + SafePath.GetFileName(f);
                }
                TempLogger.AddLog(filesLog);

                foreach (var sessionFile in files)
                {
                    try
                    {
                        var jsonData = File.ReadAllText(sessionFile);
                        var session = JsonConvert.DeserializeObject<Session>(jsonData);
                        sessionList.Add(session);
                    } catch (JsonSerializationException)
                    {
                        TempLogger.AddLog("Couldn't deserialize file : "+SafePath.GetFileName(sessionFile));
                    }
                }
                if (sessionList.Count > 0)
                {
                    var group = new GroupDisplayer
                    {
                        GroupName = sessionList[0].Level.Name,
                        GroupSessions = sessionList
                    };
                    groupList.Add(group);
                }
            }
            return groupList;
        }
    }
}
