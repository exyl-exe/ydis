using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Utilities;

namespace Whydoisuck.Model.DataStructures
{
    public class SessionGroup
    {
        [JsonProperty(PropertyName = "GroupName")] public string GroupName { get; set; }
        [JsonProperty(PropertyName = "Levels")] public List<Level> Levels { get; set; }

        [JsonIgnore] private bool _loaded = false;
        [JsonIgnore] private List<Session> groupSessions;

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

        private void LoadSessions()
        {
            groupSessions = SerializationManager.LoadGroupSessions(this);
        }

        public bool CouldContainLevel(Level level)
        {
            return Levels.Any(l => l.CouldBeSameLevel(level));
        }

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

        public void AddAndSerializeSession(Session session)
        {
            session.SessionName = GetAvailaibleSessionName(session);
            GroupSessions.Add(session);
            SerializationManager.SerializeSession(this, session);
        }

        public bool IsSessionNameAvailable(string name)
        {
            return !GroupSessions.Any(s => s.SessionName.Equals(name));
        }

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

        public static string GetDefaultGroupName(Level level)
        {
            return $"{level.Name}" + (level.Revision == 0 ? "" : $" rev{level.Revision}");
        }
    }
}
