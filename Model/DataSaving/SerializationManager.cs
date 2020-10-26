using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    static class SerializationManager
    {
        public const string SAVE_DIR = "./records/";//TODO config ?
        const string INDEX_FILE_NAME = "indexedLevels.wdis";
        public static string IndexFilePath { get { return SafePath.Combine(SAVE_DIR, INDEX_FILE_NAME); } }

        public static void SerializeSession(SessionGroup group, Session session)
        {
            var path = SafePath.Combine(GetGroupDirectoryPath(group), GetSessionFileName(session));
            Serialize(session, path);
        }

        public static void CreateGroupDirectory(SessionGroup group)
        {
            SafeDirectory.CreateDirectory(GetGroupDirectoryPath(group));
        }

        private static string GetGroupDirectoryPath(SessionGroup group)
        {
            var path = SafePath.Combine(SAVE_DIR, group.GroupName + SafePath.DirectorySeparator);
            return path;
        }
        private static string GetSessionFileName(Session session)
        {
            return session.SessionName;
        }

        public static List<Session> LoadGroupSessions(SessionGroup group)
        {
            var res = new List<Session>();
            var folderPath = GetGroupDirectoryPath(group);
            var files = SafeDirectory.GetFiles(folderPath);//TODO maybe keep a list of session files ?
            foreach (var file in files)
            {
                var session = Deserialize<Session>(file);
                res.Add(session);
            }
            return res;
        }

        public static void SerializeSessionManager(SessionManager manager)
        {
            Serialize(manager, IndexFilePath);
        }

        public static SessionManager DeserializeSessionManager()
        {
            if (!SafeFile.Exists(IndexFilePath))
            {
                return new SessionManager();
            }
            else
            {
                return Deserialize<SessionManager>(IndexFilePath);
            }
        }

        public static void Serialize(IWDISSerializable item, string filePath)
        {
            var serializedItem = item.Serialize();
            SafeFile.WriteAllText(filePath, serializedItem);
        }

        public static T Deserialize<T>(string filePath) where T : IWDISSerializable, new()
        {
            var item = new T();
            var value = SafeFile.ReadAllText(filePath);
            item.Deserialize(value);
            return item;
        }
    }
}
