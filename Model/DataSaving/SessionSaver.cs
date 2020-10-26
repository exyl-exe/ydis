using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    static class SessionSaver
    {
        public const string SAVE_DIR = "./records/";//TODO config ?
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
            var entry = new ManagerEntry() { Group = group, Level = session.Level };
            manager.AddEntry(entry);

            SerializationManager.Serialize(manager, IndexFilePath);
        }

        public static void SerializeSession(Session s, string path)
        {
            SerializationManager.Serialize(s, path);
        }

        public static Session DeserializeSession(string sessionFile)
        {
            return SerializationManager.Deserialize<Session>(sessionFile);
        }

        public static SessionManager DeserializeSessionManager()
        {
            if (!SafeFile.Exists(IndexFilePath))
            {
                return new SessionManager();
            }
            else
            {
                return SerializationManager.Deserialize<SessionManager>(IndexFilePath);
            }
        }

        public static void InitDir()
        {
            SafeDirectory.CreateDirectory(SAVE_DIR);
        }
    }
}
