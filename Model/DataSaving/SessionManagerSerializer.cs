using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.Utilities;
using Whydoisuck.Properties;

namespace Whydoisuck.DataSaving
{
    /// <summary>
    /// Manages serialization of differents objects in a given directory
    /// </summary>
    public class SessionManagerSerializer
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
                    _saveDirectory = WDISSettings.DefaultSavePath;
                }
                return _saveDirectory;
            }
            set
            {
                _saveDirectory = value;
            }
        }
        /// <summary>
        /// Path for the file containing the session manager.
        /// </summary>
        public string IndexFilePath { get { return Path.Combine(SavesDirectory, IndexFileName); } }

        // name of the file containing the session manager
        private string IndexFileName => WDISSettings.SaveManagerFileName;

        /// <summary>
        /// Initializes folders and files on the disk so that the session manager
        /// can work properly.
        /// </summary>
        public SessionManagerSerializer(string saveDir)
        {
            SavesDirectory = saveDir;
            if (!Directory.Exists(SavesDirectory))
            {
                Directory.CreateDirectory(SavesDirectory);
            }
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

        public void CopyGroupDirectory(string originalGroupName, string targetPath, string newGroupName)
        {
            var oldPath = GetGroupDirectoryPath(SavesDirectory, originalGroupName);
            var newPath = GetGroupDirectoryPath(targetPath, newGroupName);
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
            var files = Directory.GetFiles(folderPath);
            foreach (var file in files)
            {
                var session = (Session)Deserialize(file, new Session());
                res.Add(session);
            }
            return res;
        }

        /// <summary>
        /// Saves a serializable object to a given file path
        /// </summary>
        /// <param name="filePath">Where the object will be saved</param>
        /// <param name="item">The object to serialize</param>
        public void Serialize(string filePath, IWDISSerializable item)
        {
            var serializedItem = item.Serialize();
            File.WriteAllText(filePath, serializedItem);
        }

        /// <summary>
        /// Loads an object from it's file on the disk
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>The loaded object</returns>
        public IWDISSerializable Deserialize(string filePath, IWDISSerializable item)
        {
            if (!File.Exists(filePath)) return item;

            var value = File.ReadAllText(filePath);
            //Updating the object if needed
            var jo = JObject.Parse(value);
            if(!item.CurrentVersionCompatible((int)jo[IWDISSerializable.VersionPropertyName]))
            {
                item.UpdateOldVersion(ref jo);
                File.WriteAllText(filePath, jo.ToString());
            }
            item.Deserialize(jo.ToString());
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
