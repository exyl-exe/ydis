using System.Collections.Generic;
using System.Linq;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;

namespace Whydoisuck.ViewModels.DataStructures
{
    public class SessionsStatistics
    {
        private List<Session> Sessions { get; set; }
        private List<Range> Dividing { get; set; }
        private SessionFilterViewModel Filter { get; set; }
        public List<LevelPartStatistics> Statistics { get; private set; }

        public delegate void OnStatisticsChangeCallback();
        public event OnStatisticsChangeCallback OnStatisticsChange;

        public SessionsStatistics(List<Session> sessions, SessionFilterViewModel filter, float defaultPartWidth)
        {
            Sessions = sessions;
            Filter = filter;
            Dividing = GetParts(defaultPartWidth);
            Statistics = GetStatistics();
            if(Filter != null) {
                Filter.OnFilterChanges += UpdateStatistics;
            }
        }

        public SessionsStatistics(List<Session> sessions, float defaultPartWidth) : this(sessions, null, defaultPartWidth)
        {
        }

        public List<Range> GetParts(float partWidth)
        {
            var res = new List<Range>();
            var currentPartStart = 0f;//TODO const
            while(currentPartStart < 100f+partWidth)//TODO const
            {
                res.Add(new Range(currentPartStart, currentPartStart + partWidth));
                currentPartStart += partWidth;
            }
            return res;
        }

        public List<LevelPartStatistics> GetStatistics()
        {
            var res = new List<LevelPartStatistics>();
            var sessions = Filter == null ? Sessions : Sessions.Where(s => Filter.Matches(s)).ToList();
            var attempts = sessions.SelectMany(s => s.Attempts).ToList();
            var counting = new SortedList<Range, LevelPartStatistics>();

            foreach(var range in Dividing)
            {
                counting.Add(range, new LevelPartStatistics(range, 0, 0));//TODO const
            }

            SetDeathCount(attempts, counting);
            SetStartCount(sessions, counting);

            int totalReach = 0;
            foreach (var part in counting.Values)
            {
                totalReach += part.ReachCount;
                part.ReachCount = totalReach;
                totalReach -= part.DeathCount;
            }
            return counting.Select(element => element.Value).ToList();
        }

        //Set the number of death for each part in the SortedList
        public void SetDeathCount(List<Attempt> attempts, SortedList<Range, LevelPartStatistics> counting)
        {
            foreach (var attempt in attempts)
            {
                var deathRange = Dividing.Find(r => r.Contains(attempt.EndPercent));
                if (counting.TryGetValue(deathRange, out var attemptDeathPartStats))
                {
                    attemptDeathPartStats.DeathCount++;
                }
            }
        }

        //Set the reach count for each part in the SortedList to the number of attempts that started in this range
        public void SetStartCount(List<Session> sessions, SortedList<Range, LevelPartStatistics> counting)
        {
            foreach (var session in sessions)
            {
                var spawnRange = Dividing.Find(r => r.Contains(session.StartPercent));
                if (counting.TryGetValue(spawnRange, out var attemptStartPartStats))
                {
                    attemptStartPartStats.ReachCount += session.Attempts.Count;
                }
            }
        }

        private void UpdateStatistics()
        {
            Statistics = GetStatistics();
            OnStatisticsChange?.Invoke();
        }
    }
}
