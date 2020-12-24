using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.Model;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.DataSaving
{
    /// <summary>
    /// This class manages which group a session belongs to.
    /// Only one instance can exist at a given time.
    /// </summary>
    public class SessionManager : IWDISSerializable
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
                    _instance = new SessionManager();
                }
                return _instance;
            }
        }

        /// <summary>
        /// List of all groups
        /// </summary>
        [JsonProperty(PropertyName = "Groups")] public List<SessionGroup> Groups { get; set; }
        
        // Manager of the save files on the disk
        private SessionManagerSerializer Serializer { get; set; }

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
        public string SavesDirectory => Serializer.SaveDirectory;


        //private because only one instance of the class can exist
        private SessionManager()
        {
            Serializer = new SessionManagerSerializer(WDISSettings.DefaultSavePath);
            if (File.Exists(Serializer.IndexFilePath))
            {
                Serializer.Deserialize(Serializer.IndexFilePath, this);//TODO don't pass path here
                foreach(var g in Groups)
                {
                    g.SetLoader((someGroup) => Serializer.LoadGroupSessions(someGroup));
                }
            } else
            {
                Groups = new List<SessionGroup>();
            }
        }

        /// <summary>
        /// Saves the session manager on the disk.
        /// </summary>
        public void Save()
        {
            if (_instance != null)
            {
                Serializer.Serialize(Serializer.IndexFilePath, this);//TODO don't pass path here
            }
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
            var newGroup = new SessionGroup(groupName, (someGroup) => Serializer.LoadGroupSessions(someGroup));
            Groups.Add(newGroup);
            var success = Serializer.CreateGroupDirectory(newGroup);
            // This checks exists to try to correct a folder name if it's forbidden on windows
            // for instance CON, AUX ...
            // TODO Bad code
            if (!success)
            {
                newGroup.GroupName = FindAvailableGroupName(groupName + InvalidNameSuffix);
                success = Serializer.CreateGroupDirectory(newGroup);
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
            Serializer.DeleteGroupDirectory(group);
            Save();
            OnGroupDeleted?.Invoke(group);
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
            while (!IsGroupNameAvailable(groupName))
            {
                availableName = $"{groupName} ({i})";
                i++;
            }
            return availableName;
        }

        /// <summary>
        /// Serializes the session manager.
        /// </summary>
        /// <returns>A string containing a json object matching the manager.</returns>
        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Deserializes the session manager.
        /// </summary>
        /// <param name="value">A string containing a json object matching a manager.</param>
        public override void Deserialize(string value)
        {
            JsonConvert.PopulateObject(value, this);
        }

        /// <summary>
        /// Checks the version of a session manager.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public override bool CurrentVersionCompatible(int version) 
        {
            switch (IWDISSerializable.CurrentVersion)
            {
                case 2:
                    return version == 2;
                default:
                    // Prevents forgetting to update the converter
                    throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Updates an old sessionmanager object
        /// </summary>
        /// <param name="oldObject">the object to update</param>
        public override void UpdateOldVersion(ref JObject oldObject)
        {
            var version = (int)oldObject[IWDISSerializable.VersionPropertyName];
            while(!CurrentVersionCompatible(version))
            {
                switch (version)
                {
                    case 1:
                        Update1TO2ref(ref oldObject);
                        version = 2;
                        break;
                    default:
                        break;
                }
            }
        }

        //Updates from version 1 to 2
        private void Update1TO2ref(ref JObject o)
        {
            var groupsList = o["Groups"];
            foreach(var g in groupsList)
            {
                g["DisplayedName"] = g["GroupName"];
            }
        }
    }
}
