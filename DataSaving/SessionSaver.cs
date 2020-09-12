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
            File.WriteAllText(SAVE_DIR + INDEX_FILE_NAME, JsonConvert.SerializeObject(new SessionManager()));//TODO Path.Combine
        }

        public static SessionManager LoadSessionManager()
        {
            var indexerJson = File.ReadAllText(SAVE_DIR + INDEX_FILE_NAME);
            return (SessionManager)JsonConvert.DeserializeObject(indexerJson);
        }

        public static void SaveIndexer(SessionManager indexer)
        {
            var indexerUpdatedJson = JsonConvert.SerializeObject(indexer);
            File.WriteAllText(SAVE_DIR + INDEX_FILE_NAME, indexerUpdatedJson);
        }
    }
}
