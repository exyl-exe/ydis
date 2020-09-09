using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
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
            LevelIndexer indexer = LoadIndexer();
            indexer.SortBySimilarityTo(s.Level);
            if (indexer.Count != 0)
            {
                var mostSimilar = indexer[0];
                SaveUnder(indexer, mostSimilar, s);//TODO might be shit
            } else
            {
                SaveAsNewGroup(indexer, s);
            }
            SaveIndexer(indexer);
            
        }


        public static void SaveAsNewGroup(LevelIndexer indexer, Session s)
        {
            var groupName = CreateGroup(SAVE_DIR+"\\"+s.Level.Name);
            var fileName = CreateSession(groupName, s);
            var entry = new LevelEntry()
            {
                folderPath = groupName,
                fileName = fileName,
                level = s.Level
            };
            indexer.AddEntry(entry);
        }

        public static void SaveUnder(LevelIndexer indexer,LevelEntry entry, Session s)
        {
            if (entry.level.CanBeSameLevel(s.Level))
            {
                if (!entry.level.IsSameLevel(s.Level))
                {
                    var fileName = CreateSession(entry.folderPath+"\\"+s.Level.Name,s);
                    indexer.AddTwinEntry(entry, fileName, s.Level);
                }
            }
            else
            {
                SaveAsNewGroup(indexer, s);
            }
        }

        public static string CreateGroup(string groupName)
        {
            string realGroupName = groupName;
            var i = 2;
            while (Directory.Exists(realGroupName+"\\"))
            {
                realGroupName = groupName + "_" + i;
                i++;
            }
            Directory.CreateDirectory(realGroupName + "\\");
            return realGroupName;
        }

        public static string CreateSession(string path, Session s)
        {
            string realPath = path;
            var i = 2;
            while (File.Exists(realPath))
            {
                realPath = path + "_" + i;
                i++;
            }
            File.WriteAllText(realPath, JsonConvert.SerializeObject(s));
            return realPath;
        }

        public static void InitDir()
        {
            Directory.CreateDirectory(SAVE_DIR);
            File.WriteAllText(SAVE_DIR + INDEX_FILE_NAME, JsonConvert.SerializeObject(new LevelIndexer()));
        }

        public static LevelIndexer LoadIndexer()
        {
            var indexerJson = File.ReadAllText(SAVE_DIR + INDEX_FILE_NAME);
            return (LevelIndexer)JsonConvert.DeserializeObject(indexerJson);
        }

        public static void SaveIndexer(LevelIndexer indexer)
        {
            var indexerUpdatedJson = JsonConvert.SerializeObject(indexer);
            File.WriteAllText(SAVE_DIR + INDEX_FILE_NAME, indexerUpdatedJson);
        }
    }
}
