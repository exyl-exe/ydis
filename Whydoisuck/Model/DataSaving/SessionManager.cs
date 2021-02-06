using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.Model;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.UserSettings;

namespace Whydoisuck.Model.DataSaving
{
    /// <summary>
    /// This class manages which group a session belongs to.
    /// Only one instance can exist at a given time.
    /// </summary>
    public class SessionManager : WDISSerializable
    {
        private static SessionManager _instance;

        /// <summary>
        /// Sole instance of the session manager.
        /// </summary>
        public static SessionManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new SessionManager(WDISSettings.SavesPath);
                }
                return _instance;
            }
        }

        /// <summary>
        /// List of all groups
        /// </summary>
        [JsonProperty(PropertyName = "Groups")] public List<SessionGroup> Groups { get; set; }

        // Manager of the save files on the disk
        private DataSerializer Serializer { get; set; }

        /// <summary>
        /// Delegate for callbacks when a group is updated
        /// </summary>
        /// <param name="group">Updated group</param>
        public delegate void OnGroupUpdatedCallback(SessionGroup group);
        /// <summary>
        /// Event invoked when a new group is added
        /// </summary>
        public event OnGroupUpdatedCallback OnGroupUpdated;
        /// <summary>
        /// Event invoked when a group is deleted
        /// </summary>
        public event OnGroupUpdatedCallback OnGroupDeleted;
        
        /// <summary>
        /// Suffix to try to fix invalid folder names
        /// </summary>
        private static string InvalidNameSuffix => "_def";

        /// <summary>
        /// Path of the saves directory
        /// </summary>
        [JsonIgnore] public string SavesDirectory => Serializer.SavesDirectory;


        //private because only one instance of the class can exist
        private SessionManager(string path)
        {
            Init(path);
        }

        //Inits the session manager based on the data at the given path
        private void Init(string path)
        {
            Serializer = DataSerializer.CreateSerializer(path);
            bool success = File.Exists(Serializer.IndexFilePath);
            if (success)
            {
               success = success && Serializer.DeserializeSessionManager(this);
                if (success)
                {
                    foreach (var g in Groups)
                    {
                        g.SetLoader(
                            (someGroup) => Serializer.LoadGroupData(someGroup)
                         );
                    }
                }
            }
            if(!success)
            {
                Groups = new List<SessionGroup>();
            }
        }

        /// <summary>
        /// Moves where the data is stored (without moving existing data)
        /// </summary>
        public void SetRoot(string path)
        {
            if (path == SavesDirectory) return;
            foreach(var g in Groups)
            {
                OnGroupDeleted?.Invoke(g);
            }
            Groups.Clear();
            Init(path);
            foreach (var g in Groups)
            {
                OnGroupUpdated?.Invoke(g);
            }
        }

        /// <summary>
        /// Moves data from another location to current location, and merges it with existing data
        /// </summary>
        public void Import(string path)
        {
            if (path == SavesDirectory) return;
            if (!File.Exists(Path.Combine(path, DataSerializer.IndexFileName))) return;
            var otherData = new SessionManager(path);
            foreach(var g in otherData.Groups)
            {
                var originalName = g.GroupName;
                var newName = FindAvailableGroupName(originalName);
                g.GroupName = newName;
                Serializer.ImportGroup(originalName, path, newName);
            }
            Groups.AddRange(otherData.Groups);
            foreach (var g in Groups)
            {
                g.SetLoader((someGroup) => Serializer.LoadGroupData(someGroup));
            }
            Save();
            foreach (var newGroup in otherData.Groups)
            {
                OnGroupUpdated?.Invoke(newGroup);
            }
        }

        /// <summary>
        /// Saves the session manager on the disk.
        /// </summary>
        public void Save()
        {
            Serializer.SerializeSessionManager(this);
        }        

        /// <summary>
        /// Sorts groups depending on which is most likely to contain a given level
        /// </summary>
        /// <param name="level"></param>
        public void SortGroupsByClosestTo(Level level)
        {
            Groups.Sort((entry1, entry2) => Level.CompareToSample(level, entry1.GetMostSimilarLevelInGroup(level), entry2.GetMostSimilarLevelInGroup(level)));
            Groups.Reverse();
        }

        /// <summary>
        /// Saves a session in the most appropriate group
        /// </summary>
        /// <param name="session">The session to save</param>
        public void SaveSession(Session session)
        {
            // Updating session manager instance
            var group = GetOrCreateGroup(session);
            var mostSimilar = group.GetMostSimilarLevelInGroup(session.Level);
            if (mostSimilar != null && !mostSimilar.IsSameLevel(session.Level))
            {
                group.Levels.Add(session.Level);
            }
            group.AddSession(session);
            // Saving session on the disk
            Serializer.SerializeSession(group, session);
            Save();
            OnGroupUpdated?.Invoke(group);
        }

        /// <summary>
        /// Gets the most appropriate group for a session. If no existing group is appropriate, a new group is created.
        /// </summary>
        /// <param name="session">The session to get a group for</param>
        /// <returns>The most appropriate group for this session</returns>
        public SessionGroup GetOrCreateGroup(Session session)
        {
            var group = FindGroupOf(session.Level);
            if(group != null)
            {
                return group;
            }
            return CreateNewGroup(session.Level);
        }

        /// <summary>
        /// Finds what group a level belongs to
        /// </summary>
        /// <param name="level">The level to find the group of</param>
        /// <returns>The group the level belongs to</returns>
        public SessionGroup FindGroupOf(Level level)
        {
            if (Groups.Count > 0)
            {
                SortGroupsByClosestTo(level);
                var mostLikely = Groups[0];
                if (mostLikely.CouldContainLevel(level))
                {
                    return mostLikely;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a new group for a given level.
        /// </summary>
        /// <param name="level">A level to create a group for</param>
        /// <returns>The group the level will belong to</returns>
        public SessionGroup CreateNewGroup(Level level)
        {
            var defaultGroupName = SessionGroup.GetDefaultGroupName(level);
            var groupName = FindAvailableGroupName(defaultGroupName);            
            var newGroup = new SessionGroup(groupName, (someGroup) => Serializer.LoadGroupData(someGroup));
            Groups.Add(newGroup);
            var success = Serializer.SerializeGroup(newGroup);
            // This checks exists to try to correct a folder name if it's forbidden on windows
            // for instance CON, AUX ...
            // TODO Bad code
            if (!success)
            {
                newGroup.GroupName = FindAvailableGroupName(groupName + InvalidNameSuffix);
                success = Serializer.SerializeGroup(newGroup);
                if (!success) throw new Exception($"Invalid group name : '{newGroup.GroupName}'");
            }
            newGroup.Levels.Add(level);
            return newGroup;
        }

        /// <summary>
        /// Deletes the given group and its data
        /// </summary>
        /// <param name="group">The group to delete</param>
        public void DeleteGroup(SessionGroup group)
        {
            Groups.Remove(group);
            Serializer.DeleteGroup(group);
            Save();
            OnGroupDeleted?.Invoke(group);
        }        

        /// <summary>
        /// Merge the given groups and their data into a single folder
        /// </summary>
        public void MergeGroups(List<SessionGroup> allGroups)
        {
            if (allGroups.Count < 2) return;
            var root = GetMergingRoot(allGroups);
            var groupsToRemove = allGroups.Where(g => g!=root).ToList();
            foreach(var group in groupsToRemove)
            {
                root.Merge(group);
            }
            foreach(var group in groupsToRemove)
            {
                Groups.Remove(group);
                OnGroupDeleted?.Invoke(group);
            }
            OnGroupUpdated?.Invoke(root);
            // RAM data is updated before stocked data
            // so that groups to merge can still be loaded if needed
            Serializer.MergeGroups(groupsToRemove, root);
            Save();
        }

        /// <summary>
        /// Gets the group that would be the new group if given groups were merged
        /// </summary>
        public SessionGroup GetMergingRoot(List<SessionGroup> groups)
        {
            if (groups.Count() == 0) return null;
            return groups[0];
        }

        /// <summary>
        /// Reorganizes the session stored in the given folders
        /// </summary>
        public void ReorganizeGroups(List<SessionGroup> folders)
        {
            var allSessions = folders.SelectMany(f => f.GroupData.Sessions).ToList();
            //ToList() makes a copy of the list and prevents the list from being modified during the foreach
            foreach(var f in folders.ToList())
            {
                DeleteGroup(f);
            }
            foreach(var s in allSessions)
            {
                SaveSession(s);
            }
        }

        /// <summary>
        /// Reorganizes all of the data
        /// </summary>
        public void ReorganizeAll()
        {
            ReorganizeGroups(Groups);
        }

        private bool IsGroupNameAvailable(string groupName)
        {
            foreach (var group in Groups)
            {
                if (group.GroupName.ToUpper().Equals(groupName.ToUpper()))
                {
                    return false;
                }
            }
            return true;
        }

        private string FindAvailableGroupName(string groupName)
        {
            var i = 2;
            var availableName = groupName;
            while (!IsGroupNameAvailable(availableName))
            {
                availableName = $"{groupName} ({i})";
                i++;
            }
            return availableName;
        }
    }
}
