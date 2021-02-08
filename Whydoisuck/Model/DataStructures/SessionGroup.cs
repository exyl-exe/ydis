using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

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
        /// Name of the level that is displayed
        /// </summary>
        [JsonProperty(PropertyName = "DisplayedName")] public string DisplayedName {
            get
            {
                return displayedName;
            }
            set
            {
                displayedName = value;
                OnDisplayedNameChanges?.Invoke();
            }
        }
        /// <summary>
        /// When the level of the group was last played
        /// </summary>
        [JsonProperty(PropertyName = "LastPlayed")] public DateTime LastPlayedTime { get; set; }
        /// <summary>
        /// List of individual levels (aka copies) the sessions were played on.
        /// </summary>
        [JsonProperty(PropertyName = "Levels")] public List<Level> Levels { get; set; }
        /// <summary>
        /// Data of the group (for statistics, can be loaded independently)
        /// </summary>
        [JsonIgnore]
        public SessionGroupData GroupData {
            get
            {
                if (!_loaded)
                {
                    groupData = _loader(this);
                    _loaded = true;
                }
                return groupData;
            }
            set
            {
                groupData = value;
            }
        }
        // false if the sessions weren't loaded yet
        // exists to avoid loading every session in a group if they are not accessed
        [JsonIgnore] private bool _loaded = false;
        // List of sessions in the group, null if not loaded.
        [JsonIgnore] private SessionGroupData groupData;
        //Function to call when the group needs to be loaded
        [JsonIgnore] private Func<SessionGroup, SessionGroupData> _loader;

        /// <summary>
        /// Delegate for callbacks when the displayed name is updated
        /// </summary>
        public delegate void UpdateDelegate();
        /// <summary>
        /// Invoked when the displayed name changes
        /// </summary>
        public event UpdateDelegate OnDisplayedNameChanges;
        /// <summary>
        /// Invoked when a session is added or removed from the group
        /// </summary>
        public event UpdateDelegate OnSessionsChange;
        //displayed name property
        [JsonIgnore] private string displayedName;

        public SessionGroup(string name, Func<SessionGroup,SessionGroupData> dataLoader)
        {
            _loader = dataLoader;
            GroupName = name;
            DisplayedName = name;
            Levels = new List<Level>();
        }

        /// <summary>
        /// Merges the given group into this group
        /// </summary>
        public void Merge(SessionGroup group)
        {
            if (group.LastPlayedTime > LastPlayedTime)
            {
                LastPlayedTime = group.LastPlayedTime;
            }

            foreach(var level in group.Levels)
            {
                if(!Levels.Any(l => l.IsSameLevel(level)))
                {
                    Levels.Add(level);
                }
            }

            GroupData.Merge(group.GroupData);
            OnSessionsChange?.Invoke();
        }

        /// <summary>
        /// Sets a method to call when the group needs to be loaded
        /// </summary>
        public void SetLoader(Func<SessionGroup, SessionGroupData> loader)
        {
            _loader = loader;
        }

        /// <summary>
        /// Adds a session to the group. 
        /// </summary>
        /// <param name="session">Session to add</param>
        public void AddSession(Session session)
        {
            if (LastPlayedTime < session.StartTime) LastPlayedTime = session.StartTime;
            GroupData.AddSession(session);
            OnSessionsChange?.Invoke();
        }

        /// <summary>
        /// Checks if an individual level is already in the group.
        /// It means some sessions in the group were played on the level.
        /// </summary>
        /// <param name="level">Individual level that might already be in the group</param>
        /// <returns>True if the level is in the group, false otherwise</returns>
        public bool CouldContainLevel(Level level)
        {
            return Levels.Any(l => l.ShouldBeGroupedWith(level));
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
        /// Gets the default name for a group containing a given level.
        /// </summary>
        /// <param name="level">The level that would be in the group.</param>
        /// <returns>The name the group would have if it contained the given level.</returns>
        public static string GetDefaultGroupName(Level level)
        {
            return $"{level.Name}" + (level.Revision == 0 ? "" : $" rev{level.Revision}");
        }
    }
}
