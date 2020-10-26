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
        [JsonIgnore] public List<Session> GroupSessions { get; set; }//TODO shouldn't be loaded directly

        public SessionGroup() {} //For json deserialization

        public SessionGroup(string name)
        {
            GroupName = name;
            Levels = new List<Level>();
            GroupSessions = new List<Session>();
        }

        public bool CouldContainLevel(Level level)
        {
            return Levels.Any(l => l.CouldBeSameLevel(level));
        }

        public Level GetMostSimilarLevelInGroup(Level level)
        {
            if(Levels.Count > 0)
            {
                var mostSimilar = Levels[0];
                foreach (var l in Levels)
                {
                    if(Level.CompareToSample(level, mostSimilar, l)< 0)
                    {
                        mostSimilar = l;
                    }
                }
                return mostSimilar;
            } else
            {
                return null;
            }
        }

        public void AddSession(Session session)
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
            while (!IsSessionNameAvailable(name)) //TODO might be slow
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
