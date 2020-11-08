using System.Collections.Generic;
using System.Linq;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;

namespace Whydoisuck.ViewModels.DataStructures
{
    /// <summary>
    /// Class that computes statistics about each part of a level, based on a list of sessions.
    /// </summary>
    public class SessionsStatistics
    {
        /// <summary>
        /// Statistics about every part of the level
        /// </summary>
        public List<LevelPartStatistics> Statistics { get; private set; }

        /// <summary>
        /// Delegate for callbacks when the statistics are updated.
        /// </summary>
        public delegate void OnStatisticsChangeCallback();
        /// <summary>
        /// Invoked when the statistics about these sessions change.
        /// </summary>
        public event OnStatisticsChangeCallback OnStatisticsChange;
        // All sessions that can contribute to the statistics.
        private List<Session> Sessions { get; set; }
        // How the level is divided. These ranges must not overlap.
        private List<Range> Dividing { get; set; }
        // Criteria for selecting sessions to compute statistics about.
        private SessionFilterViewModel Filter { get; set; }

        const double MIN_PERCENT = 0;
        const double MAX_PERCENT = 100;

        public SessionsStatistics(List<Session> sessions, SessionFilterViewModel filter, double defaultPartWidth)
        {
            Sessions = sessions;
            Filter = filter;
            Dividing = GetParts(defaultPartWidth);
            Statistics = GetStatistics();
            if(Filter != null) {
                Filter.OnFilterChanges += UpdateStatistics;
            }
        }

        public SessionsStatistics(List<Session> sessions, double defaultPartWidth) : this(sessions, null, defaultPartWidth)
        {
        }

        /// <summary>
        /// Gives ranges that divide the level.
        /// </summary>
        /// <param name="partWidth">Width of each range.</param>
        /// <returns>The list of ranges that divide the level.</returns>
        public List<Range> GetParts(double partWidth)
        {
            var res = new List<Range>();
            var currentPartStart = MIN_PERCENT;
            while(currentPartStart < MAX_PERCENT+partWidth)
            {
                res.Add(new Range(currentPartStart, currentPartStart + partWidth));
                currentPartStart += partWidth;
            }
            return res;
        }

        /// <summary>
        /// Gets statistics about the level, taking into account filtering criteria.
        /// </summary>
        /// <returns>Statistics about each part of the level</returns>
        public List<LevelPartStatistics> GetStatistics()
        {
            var res = new List<LevelPartStatistics>();
            // Sessions matching the filter
            var sessions = Filter == null ? Sessions : Sessions.Where(s => Filter.Matches(s)).ToList();
            // All attempts of valid sessions
            var attempts = sessions.SelectMany(s => s.Attempts).ToList();
            var counting = new SortedList<Range, LevelPartStatistics>();

            foreach(var range in Dividing)
            {
                counting.Add(range, new LevelPartStatistics(range));
            }

            SetDeathCount(attempts, counting);
            SetStartCount(sessions, counting);

            int totalReach = 0;
            // This computes actual reach count, by adding start count and
            // removing death count of the current part for each part.
            foreach (var part in counting.Values)
            {
                totalReach += part.ReachCount;
                part.ReachCount = totalReach;
                totalReach -= part.DeathCount;
            }
            return counting.Select(element => element.Value).ToList();
        }

        // Set the number of death for each part in the SortedList
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

        // Set the reach count for each part in the SortedList to the number of attempts that started in this range
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

        // Updates statistics and notify the change 
        private void UpdateStatistics()
        {
            Statistics = GetStatistics();
            OnStatisticsChange?.Invoke();
        }
    }
}
