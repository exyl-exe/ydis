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

            SessionManager manager = LoadSessionManager();//TODO only do that like once, and not each time there is a session to save (athough it doesn't matter much)
            var group = manager.GetGroup(s);
            var entry = group.AddSession(s);
            manager.AddEntry(entry);
            SaveIndexer(manager);
        }

        public static void InitDir()
        {
            Directory.CreateDirectory(SAVE_DIR);
            File.WriteAllText(SAVE_DIR + INDEX_FILE_NAME, JsonConvert.SerializeObject(new SessionManager(), Formatting.Indented));//TODO Path.Combine
        }

        public static SessionManager LoadSessionManager()
        {
            if(!File.Exists(SAVE_DIR + INDEX_FILE_NAME))
            {
                return new SessionManager();
            } else
            {
                SessionManager storedManager;
                var indexerJson = File.ReadAllText(SAVE_DIR + INDEX_FILE_NAME);
                storedManager = JsonConvert.DeserializeObject<SessionManager>(indexerJson);
                return storedManager;
            } 
        }

        public static void SaveIndexer(SessionManager manager)
        {
            var indexerUpdatedJson = JsonConvert.SerializeObject(manager, Formatting.Indented);
            File.WriteAllText(SAVE_DIR + INDEX_FILE_NAME, indexerUpdatedJson);
        }
    }
}
