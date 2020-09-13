using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.DataSaving
{
    class SessionManager
    {
        public List<IndexerEntry> Entries { get; set; }

        public SessionManager()
        {
            Entries = new List<IndexerEntry>();
        }

        public void SortEntriesBySimilarityTo(Level level)
        {
            Entries.Sort((entry1, entry2) => Level.LevelComparison(level, entry1.Level, entry2.Level));
            Entries.Reverse();
        }

        public void AddEntry(IndexerEntry entry)
        {
            foreach(var existingEntry in Entries)
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
            if(Entries.Count > 0)
            {
                SortEntriesBySimilarityTo(session.Level);
                var mostLikely = Entries[0];
                if (mostLikely.Level.CouldBeSameLevel(session.Level))
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
            var newGroup = new SessionGroup() { Name = groupName };
            newGroup.CreateGroupDirectory();
            return newGroup;
        }

        private bool IsGroupNameAvailable(string groupName)//TODO potentially laggy if called repeatedly, might be worth to sort list OR to check save directory directly
        {
            foreach(var e in Entries)
            {
                if (e.Group.Name.Equals(groupName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
