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
    public class SessionManager : IWDISSerializable
    {
        [JsonProperty(PropertyName = "Groups")] public List<SessionGroup> Groups { get; set; }

        public SessionManager()
        {
            Groups = new List<SessionGroup>();
        }

        public void SortGroupsByClosestTo(Level level)
        {
            //TODO giga bad because most similar level in group is computed several time
            Groups.Sort((entry1, entry2) => Level.CompareToSample(level, entry1.GetMostSimilarLevelInGroup(level), entry2.GetMostSimilarLevelInGroup(level)));
            Groups.Reverse();
        }

        public void SaveSession(Session session)
        {
            var group = GetOrCreateGroup(session);
            if (!group.GetMostSimilarLevelInGroup(session.Level).IsSameLevel(session.Level))
            {
                group.Levels.Add(session.Level);
            }
            group.AddAndSerializeSession(session);
        }

        public SessionGroup GetOrCreateGroup(Session session)
        {
            if (Groups.Count > 0)
            {
                SortGroupsByClosestTo(session.Level);
                var mostLikely = Groups[0];
                if (mostLikely.CouldContainLevel(session.Level))
                {
                    return mostLikely;
                }
            }
            return CreateNewGroup(session.Level);
        }

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

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public override void Deserialize(string value)
        {
            JsonConvert.PopulateObject(value, this);
        }
    }
}
