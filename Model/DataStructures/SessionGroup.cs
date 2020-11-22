using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using Whydoisuck.DataSaving;
using Whydoisuck.Utilities;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// Group of sessions played on the same level
    /// </summary>
    public class SessionGroup
    {
        /// <summary>
        /// Name of the level the sessions were played on.
        /// </summary>
        [JsonProperty(PropertyName = "GroupName")] public string GroupName { get; set; }
        /// <summary>
        /// List of individual levels (aka copies) the sessions were played on.
        /// </summary>
        [JsonProperty(PropertyName = "Levels")] public List<Level> Levels { get; set; }
        /// <summary>
        /// When the level of the group was last played
        /// </summary>
        [JsonProperty(PropertyName = "LastPlayed")] public DateTime LastPlayedTime { get; set; }

        // false if the sessions weren't loaded yet
        // exists to avoid loading every session in a group if they are not accessed
        [JsonIgnore] private bool _loaded = false;
        // List of sessions in the group, null if not loaded.
        [JsonIgnore] private List<Session> groupSessions;

        /// <summary>
        /// List of all the sessions played on the level.
        /// </summary>
        [JsonIgnore] public List<Session> GroupSessions
        {
            get
            {
                if (!_loaded)
                {
                    LoadSessions();
                    _loaded = true;
                }
                return groupSessions;
            }
            set
            {
                groupSessions = value;
            }
        }

        public SessionGroup() { } //For json deserialization

        public SessionGroup(string name)
        {
            GroupName = name;
            Levels = new List<Level>();
            GroupSessions = new List<Session>();
        }

        /// <summary>
        /// Adds a session to the group and saves it on the disk. 
        /// </summary>
        /// <param name="session">Session to add and serialize</param>
        public void AddAndSerializeSession(Session session)
        {
            if (LastPlayedTime < session.StartTime) LastPlayedTime = session.StartTime;
            session.SessionName = GetAvailaibleSessionName(session);
            GroupSessions.Add(session);
            SerializationManager.SerializeSession(this, session);
        }

        /// <summary>
        /// Checks if an individual level is already in the group.
        /// It means some sessions in the group were played on the level.
        /// </summary>
        /// <param name="level">Individual level that might already be in the group</param>
        /// <returns>True if the level is in the group, false otherwise</returns>
        public bool CouldContainLevel(Level level)
        {
            return Levels.Any(l => l.CouldBeSameLevel(level));
        }

        /// <summary>
        /// Get the most similar level in the group to the given level
        /// </summary>
        /// <param name="level">Level to look for</param>
        /// <returns>The most similar level to the parameter.
        /// Null if there aren't any level in the group.</returns>
        public Level GetMostSimilarLevelInGroup(Level level)
        {
            if (Levels.Count > 0)
            {
                var mostSimilar = Levels[0];
                foreach (var l in Levels)
                {
                    if (Level.CompareToSample(level, mostSimilar, l) < 0)
                    {
                        mostSimilar = l;
                    }
                }
                return mostSimilar;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if the given name doesn't match any session in the group
        /// </summary>
        /// <param name="name">Name to check availability for</param>
        /// <returns>True if the name is available, false otherwise</returns>
        public bool IsSessionNameAvailable(string name)
        {
            return !GroupSessions.Any(s => s.SessionName.Equals(name));
        }

        /// <summary>
        /// Gets an available name for the given session.
        /// </summary>
        /// <param name="session">Session that will be named</param>
        /// <returns>An available name matching the session</returns>
        public string GetAvailaibleSessionName(Session session)
        {
            var defaultName = session.GetDefaultSessionFileName();
            var name = defaultName;
            var i = 2;
            while (!IsSessionNameAvailable(name))
            {
                name = $"{defaultName} ({i})";
                i++;
            }
            return name;
        }

        /// <summary>
        /// Gets the default name for a group containing a given level.
        /// </summary>
        /// <param name="level">The level that would be in the group.</param>
        /// <returns>The name the group would have if it contained the given level.</returns>
        public static string GetDefaultGroupName(Level level)
        {
            return $"{level.Name}" + (level.Revision == 0 ? "" : $" rev{level.Revision}");
        }

        // Load sessions in the group. Needed because of how sessions are saved.
        private void LoadSessions()
        {
            groupSessions = SerializationManager.LoadGroupSessions(this);
        }
    }
}
