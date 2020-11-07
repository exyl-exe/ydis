using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.DataSaving
{
    /// <summary>
    /// This class manages which group a session belongs to
    /// </summary>
    public class SessionManager : IWDISSerializable
    {
        /// <summary>
        /// List of all groups
        /// </summary>
        [JsonProperty(PropertyName = "Groups")] public List<SessionGroup> Groups { get; set; }

        /// <summary>
        /// Delegate for callbacks when a group is updated
        /// </summary>
        /// <param name="group">Updated group</param>
        public delegate void OnGroupUpdatedCallback(SessionGroup group);
        /// <summary>
        /// Event invoked when a new group is added
        /// </summary>
        public event OnGroupUpdatedCallback OnGroupUpdated;

        public SessionManager()
        {
            Groups = new List<SessionGroup>();
        }

        /// <summary>
        /// Sorts groups depending on which is most likely to contain a given level
        /// </summary>
        /// <param name="level"></param>
        public void SortGroupsByClosestTo(Level level)
        {
            //TODO giga bad because most similar level in group is computed several time
            Groups.Sort((entry1, entry2) => Level.CompareToSample(level, entry1.GetMostSimilarLevelInGroup(level), entry2.GetMostSimilarLevelInGroup(level)));
            Groups.Reverse();
        }

        /// <summary>
        /// Saves a session in the most appropriate group
        /// </summary>
        /// <param name="session">The session to save</param>
        public void SaveSession(Session session)
        {
            var group = GetOrCreateGroup(session);
            if (!group.GetMostSimilarLevelInGroup(session.Level).IsSameLevel(session.Level))
            {
                group.Levels.Add(session.Level);
            }
            group.AddAndSerializeSession(session);
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
            var groupName = defaultGroupName;
            var i = 2;
            while (!IsGroupNameAvailable(groupName))
            {
                groupName = $"{defaultGroupName} ({i})";
                i++;
            }
            var newGroup = new SessionGroup(groupName);
            Groups.Add(newGroup);
            newGroup.Levels.Add(level);
            SerializationManager.CreateGroupDirectory(newGroup);
            return newGroup;
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

        private bool IsGroupNameAvailable(string groupName)
        {
            foreach (var group in Groups)
            {
                if (group.GroupName.Equals(groupName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
