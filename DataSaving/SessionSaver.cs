using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Recording;

namespace Whydoisuck.DataSaving
{
    static class SessionSaver
    {
        const string SAVE_DIR = "./records/";
        const string INDEX_FILE_NAME = "indexedLevels.wdis";

        public static void SaveSession(Session s)
        {
            if (!Directory.Exists(SAVE_DIR))
            {
                InitDir();
            }
            //File.WriteAllText(SAVE_DIR+"test.json",json);TODO
            LevelIndexer indexer = (LevelIndexer)JsonConvert.DeserializeObject(SAVE_DIR + INDEX_FILE_NAME);
            indexer.SortBySimilarityTo(s.Level);
            var json = JsonConvert.SerializeObject(s, Formatting.Indented);
            if (indexer.Count != 0)
            {
                var mostSimilar = indexer[0];
                SaveUnder(mostSimilar, s);
            } else
            {
                SaveAsNewGroup(s);
            }
        }

        private static void SaveAsNewGroup(Session s)
        {
            throw new NotImplementedException();
        }

        private static void SaveUnder(LevelEntry entry, Session s)
        {
            if(entry.level.IsSameLevel())
        }

        public static void InitDir()
        {
            Directory.CreateDirectory(SAVE_DIR);
            File.WriteAllText(SAVE_DIR + INDEX_FILE_NAME, JsonConvert.SerializeObject(new LevelIndexer()));
        }
    }
}
