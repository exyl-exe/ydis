using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Model;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.UserSettings;
using Whydoisuck.Model.Utilities;
using Whydoisuck.Properties;

namespace Whydoisuck.Model.DataSaving
{
    /// <summary>
    /// Manages serialization of differents objects in a given directory
    /// </summary>
    public class DataSerializer
    {
        private string _saveDirectory = null;
        /// <summary>
        /// Directory where all of the data is saved.
        /// </summary>
        public string SavesDirectory {
            get
            {
                if(_saveDirectory == null)
                {
                    _saveDirectory = WDISSettings.SavesPath;
                }
                return _saveDirectory;
            }
            set
            {
                _saveDirectory = value;
            }
        }
        /// <summary>
        /// name of the file containing the session manager
        /// </summary>
        public static string IndexFileName => WDISSettings.SaveManagerFileName;
        /// <summary>
        /// Path for the file containing the session manager.
        /// </summary>
        public string IndexFilePath { get { return Path.Combine(SavesDirectory, IndexFileName); } }

        private DataSerializer(string dir)
        {
            SavesDirectory = dir;
        }

        /// <summary>
        /// Initializes folders and files on the disk so that the session manager
        /// can work properly.
        /// </summary>
        public static DataSerializer CreateSerializer(string saveDir)
        {
            Reformat(saveDir);
            var ser = new DataSerializer(saveDir);
            return ser;
        }

        // Formats the data at the given path so it can be used by a DataSerializer
        private static void Reformat(string saveDir)
        {
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            else
            {
                DataUpdater.Update(saveDir);
            }
        }

        /// <summary>
        /// Saves the session manager on the disk
        /// </summary>
        public void SerializeSessionManager(SessionManager manager)
        {
            Serialize(IndexFilePath, manager);
        }

        /// <summary>
        /// Deserializes the session manager
        /// </summary>
        public void DeserializeSessionManager(SessionManager manager)
        {
            Deserialize(IndexFilePath, manager);
        }

        /// <summary>
        /// Saves a session on the disk
        /// </summary>
        /// <param name="group">The group the session belongs to</param>
        /// <param name="session">The session to save</param>
        public void SerializeSession(SessionGroup group, Session session)
        {
            var path = Path.Combine(GetGroupDirectoryPath(group), GetSessionFileName(session));
            Serialize(path, session);
        }

        /// <summary>
        /// Creates a directory for a given group
        /// </summary>
        /// <param name="group">The group to create a directory for</param>
        public bool CreateGroupDirectory(SessionGroup group)
        {
            var path = GetGroupDirectoryPath(group);
            try
            {
                Directory.CreateDirectory(path);
                return true;
            } catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Imports a group from another save folder
        /// </summary>
        /// <param name="originalGroupName">Name of the group in the saves to import</param>
        /// <param name="targetPath">Path of the saves to import</param>
        /// <param name="newGroupName">New name the group will have in the current data</param>
        public void ImportGroupDirectory(string originalGroupName, string targetPath, string newGroupName)
        {
            var oldPath = GetGroupDirectoryPath(targetPath, originalGroupName);
            var newPath = GetGroupDirectoryPath(SavesDirectory, newGroupName);
            DirectoryUtilities.Copy(oldPath, newPath, true);
        }

        /// <summary>
        /// Deletes the directory associated to a group and all of its content
        /// </summary>
        /// <param name="group"></param>
        public void DeleteGroupDirectory(SessionGroup group)
        {
            var path = GetGroupDirectoryPath(group);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Loads the sessions of a group
        /// </summary>
        /// <param name="group">The group to load the sessions of</param>
        /// <returns>The sessions of the group</returns>
        public List<Session> LoadGroupSessions(SessionGroup group)
        {
            var res = new List<Session>();
            var folderPath = GetGroupDirectoryPath(group);
            if(!Directory.Exists(folderPath))
            {
                CreateGroupDirectory(group);
                return res;
            }
            var files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                try
                {
                    var session = (Session)Deserialize(file, new Session());
                    res.Add(session);
                }
                catch (JsonReaderException) { }                
            }
            return res;
        }

        /// <summary>
        /// Merges all the directories of the given groups into the root's directory
        /// </summary>
        public void MergeGroupsDirectories(List<SessionGroup> groups, SessionGroup root)
        {
            var newPath = GetGroupDirectoryPath(root);
            foreach(var group in groups)
            {
                DirectoryUtilities.MoveDirectoryContent(GetGroupDirectoryPath(group), newPath);
            }
        }

        /// <summary>
        /// Saves a serializable object to a given file path
        /// </summary>
        /// <param name="filePath">Where the object will be saved</param>
        /// <param name="item">The object to serialize</param>
        private void Serialize(string filePath, WDISSerializable item)
        {
            var serializedItem = item.ToJsonObject().ToString();
            File.WriteAllText(filePath, serializedItem);
        }

        /// <summary>
        /// Loads an object from it's file on the disk
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>The loaded object</returns>
        private WDISSerializable Deserialize(string filePath, WDISSerializable item)
        {
            if (!File.Exists(filePath)) return item;
            var value = File.ReadAllText(filePath);
            var jo = JObject.Parse(value);
            item.FromJsonObject(jo);
            return item;
        }

        // Gets the path of the directory of a group.
        private string GetGroupDirectoryPath(SessionGroup group)
        {
            return GetGroupDirectoryPath(SavesDirectory, group.GroupName);
        }

        // Gets the path of the directory of a group, with a given a root.
        private string GetGroupDirectoryPath(string rootPath, string groupName)
        {
            var path = Path.Combine(rootPath, groupName.Trim());
            return path;
        }

        // Gets to file name for a session
        private string GetSessionFileName(Session session)
        {
            return session.SessionName;
        }
    }
}
