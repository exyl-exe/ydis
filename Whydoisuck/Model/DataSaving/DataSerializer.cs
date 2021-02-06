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
        public bool DeserializeSessionManager(SessionManager manager)
        {
            try
            {
                Deserialize(IndexFilePath, manager);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        /// <summary>
        /// Saves a session on the disk
        /// </summary>
        /// <param name="group">The group the session belongs to</param>
        /// <param name="session">The session to save</param>
        public void SerializeSession(SessionGroup group, Session session)
        {
            SerializeGroup(group);
        }

        /// <summary>
        /// Serializes a group
        /// </summary>
        public bool SerializeGroup(SessionGroup group)
        {
            var path = GetGroupDataPath(group);
            try
            {
                Serialize(path, group.GroupData);
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
        public void ImportGroup(string originalGroupName, string targetPath, string newGroupName)
        {
            var oldPath = GetGroupDataPath(targetPath, originalGroupName);
            var newPath = GetGroupDataPath(SavesDirectory, newGroupName);
            File.Copy(oldPath, newPath);
        }

        /// <summary>
        /// Deletes the group and its content from the disk
        /// </summary>
        public void DeleteGroup(SessionGroup group)
        {
            var path = GetGroupDataPath(group);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Loads the data of a group
        /// </summary>
        public SessionGroupData LoadGroupData(SessionGroup group)
        {
            SessionGroupData res = new SessionGroupData();
            var folderPath = GetGroupDataPath(group);
            if (File.Exists(folderPath))
            {
                Deserialize(folderPath, res);
            }
            return res;
        }

        /// <summary>
        /// Merges all the given groups into the root group
        /// </summary>
        public void MergeGroups(List<SessionGroup> groups, SessionGroup root)//TODO
        {
            var newPath = GetGroupDataPath(root);
            foreach(var group in groups)
            {
                DirectoryUtilities.MoveDirectoryContent(GetGroupDataPath(group), newPath);
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

        // Gets the path of the data of a group.
        private string GetGroupDataPath(SessionGroup group)
        {
            return GetGroupDataPath(SavesDirectory, group.GroupName);
        }

        // Gets the path of the data of a group, with a given save directory.
        private string GetGroupDataPath(string rootPath, string groupName)
        {
            var path = Path.Combine(rootPath, groupName.Trim());
            return path;
        }
    }
}
