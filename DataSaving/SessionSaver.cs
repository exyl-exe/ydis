using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Whydoisuck.DataModel;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    static class SessionSaver
    {
        public const string SAVE_DIR = "./records/";
        const string INDEX_FILE_NAME = "indexedLevels.wdis";
        public static string IndexFilePath { get { return SafePath.Combine(SAVE_DIR, INDEX_FILE_NAME); } }

        public static void SaveSession(Session session)
        {
            if (!SafeDirectory.Exists(SAVE_DIR))
            {
                InitDir();
            }
            SessionManager manager = DeserializeSessionManager();

            var group = manager.GetGroup(session);
            group.AddSession(session);
            var entry = new IndexerEntry() { Group = group, Level = session.Level };
            manager.AddEntry(entry);

            SerializeSessionManager(manager);
        }

        public static void SerializeSession(Session s, string path)
        {
            var sessionJson = JsonConvert.SerializeObject(s, Formatting.Indented);
            SafeFile.WriteAllText(path, sessionJson);
        }

        public static Session DeserializeSession(string sessionFile)
        {
            var jsonData = File.ReadAllText(sessionFile);
            return JsonConvert.DeserializeObject<Session>(jsonData);
        }

        public static void InitDir()
        {
            SafeDirectory.CreateDirectory(SAVE_DIR);
            SafeFile.WriteAllText(IndexFilePath, JsonConvert.SerializeObject(new SessionManager(), Formatting.Indented));
        }

        public static SessionManager DeserializeSessionManager()
        {
            if (!SafeFile.Exists(IndexFilePath))
            {
                return new SessionManager();
            }
            else
            {
                SessionManager storedManager;
                var indexerJson = SafeFile.ReadAllText(IndexFilePath);
                storedManager = JsonConvert.DeserializeObject<SessionManager>(indexerJson);
                return storedManager;
            }
        }

        public static void SerializeSessionManager(SessionManager manager)
        {
            var indexerUpdatedJson = JsonConvert.SerializeObject(manager, Formatting.Indented);
            SafeFile.WriteAllText(IndexFilePath, indexerUpdatedJson);
        }
    }
}
