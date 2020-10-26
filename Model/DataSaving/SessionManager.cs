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
    class SessionManager : IWDISSerializable
    {
        [JsonProperty(PropertyName = "Entries")] public List<ManagerEntry> Entries { get; set; }

        public SessionManager()
        {
            Entries = new List<ManagerEntry>();
        }

        public void SortEntriesBySimilarityTo(Level level)
        {
            Entries.Sort((entry1, entry2) => Level.LevelComparison(level, entry1.Level, entry2.Level));
            Entries.Reverse();
        }

        public void AddEntry(ManagerEntry entry)
        {
            foreach (var existingEntry in Entries)
            {
                if (existingEntry.SameEntry(entry))
                {
                    return;
                }
            }
            Entries.Add(entry);
        }

        public SessionGroup GetGroup(Session session)
        {
            if (Entries.Count > 0)
            {
                SortEntriesBySimilarityTo(session.Level);
                var mostLikely = Entries[0];
                var sameLevel = mostLikely.Level.CouldBeSameLevel(session.Level);
                if (sameLevel)
                {
                    return mostLikely.Group;
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
            var newGroup = new SessionGroup() { GroupName = groupName };
            newGroup.CreateGroupDirectory();
            return newGroup;
        }

        private bool IsGroupNameAvailable(string groupName)//TODO potentially laggy if called repeatedly, might be worth to sort list OR to check save directory directly
        {
            foreach (var e in Entries)
            {
                if (e.Group.GroupName.Equals(groupName))
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
