using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Whydoisuck.DataSaving;

namespace Whydoisuck.DataSaving
{
    static class SessionSaver
    {
        public const string SAVE_DIR = "./records/";
        const string INDEX_FILE_NAME = "indexedLevels.wdis";
        public static string IndexFilePath { get { return Path.Combine(SAVE_DIR, INDEX_FILE_NAME); } }

        public static void SaveSession(Session s)
        {
            if (!Directory.Exists(SAVE_DIR))
            {
                InitDir();
            }

            SessionManager manager = LoadSessionManager();
            var group = manager.GetGroup(s);
            var entry = group.AddSession(s);
            manager.AddEntry(entry);
            SaveIndexer(manager);
        }

        public static void InitDir()
        {
            Directory.CreateDirectory(SAVE_DIR);
            File.WriteAllText(IndexFilePath, JsonConvert.SerializeObject(new SessionManager(), Formatting.Indented));
        }

        public static SessionManager LoadSessionManager()
        {
            if (!File.Exists(IndexFilePath))
            {
                return new SessionManager();
            } else
            {
                SessionManager storedManager;
                var indexerJson = File.ReadAllText(IndexFilePath);
                storedManager = JsonConvert.DeserializeObject<SessionManager>(indexerJson);
                return storedManager;
            } 
        }

        public static void SaveIndexer(SessionManager manager)
        {
            var indexerUpdatedJson = JsonConvert.SerializeObject(manager, Formatting.Indented);
            File.WriteAllText(IndexFilePath, indexerUpdatedJson);
        }
    }
}
