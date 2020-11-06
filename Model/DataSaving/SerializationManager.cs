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
    /// <summary>
    /// Manages serialization of differents objects.
    /// </summary>
    public static class SerializationManager
    {
        /// <summary>
        /// Directory where all of the data is saved.
        /// </summary>
        public const string SAVE_DIR = "./records/";//TODO config ?
        /// <summary>
        /// Path for the file containing the session manager.
        /// </summary>
        public static string IndexFilePath { get { return SafePath.Combine(SAVE_DIR, INDEX_FILE_NAME); } }

        // name of the file containing the session manager
        const string INDEX_FILE_NAME = "indexedLevels.wdis";

        /// <summary>
        /// Saves a session on the disk
        /// </summary>
        /// <param name="group">The group the session belongs to</param>
        /// <param name="session">The session to save</param>
        public static void SerializeSession(SessionGroup group, Session session)
        {
            var path = SafePath.Combine(GetGroupDirectoryPath(group), GetSessionFileName(session));
            Serialize(session, path);
        }

        /// <summary>
        /// Creates a directory for a given group
        /// </summary>
        /// <param name="group">The group to create a directory for</param>
        public static void CreateGroupDirectory(SessionGroup group)
        {
            SafeDirectory.CreateDirectory(GetGroupDirectoryPath(group));
        }

        /// <summary>
        /// Loads the sessions of a group
        /// </summary>
        /// <param name="group">The group to load the sessions of</param>
        /// <returns>The sessions of the group</returns>
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

        /// <summary>
        /// Saves the session manager on the disk
        /// </summary>
        /// <param name="manager"></param>
        public static void SerializeSessionManager(SessionManager manager)
        {
            Serialize(manager, IndexFilePath);
        }

        /// <summary>
        /// Loads the manager from the file on the disk
        /// </summary>
        /// <returns>The loaded session manager</returns>
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

        /// <summary>
        /// Saves a serializable object to a given file path
        /// </summary>
        /// <param name="item">The object to serialize</param>
        /// <param name="filePath">Where the object will be saved</param>
        public static void Serialize(IWDISSerializable item, string filePath)
        {
            var serializedItem = item.Serialize();
            SafeFile.WriteAllText(filePath, serializedItem);
        }

        /// <summary>
        /// Loads an object from it's file on the disk
        /// </summary>
        /// <typeparam name="T">Type of the saved object</typeparam>
        /// <param name="filePath">Path to the file</param>
        /// <returns>The loaded object</returns>
        public static T Deserialize<T>(string filePath) where T : IWDISSerializable, new()
        {
            var item = new T();
            var value = SafeFile.ReadAllText(filePath);
            item.Deserialize(value);
            return item;
        }

        // Gets the path of the directory of a group.
        private static string GetGroupDirectoryPath(SessionGroup group)
        {
            var path = SafePath.Combine(SAVE_DIR, group.GroupName + SafePath.DirectorySeparator);
            return path;
        }

        // Gets to file name for a session
        private static string GetSessionFileName(Session session)
        {
            return session.SessionName;
        }
    }
}
