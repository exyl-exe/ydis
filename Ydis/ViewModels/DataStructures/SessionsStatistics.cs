using System;
using System.Collections.Generic;
using System.Linq;
using Ydis.Model.DataStructures;
using Ydis.ViewModels.CommonControlsViewModels;

namespace Ydis.ViewModels.DataStructures
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
        /// Total playtime in the sessions
        /// </summary>
        public TimeSpan PlayTime { get; set; }
        /// <summary>
        /// Total attempts in the sessions
        /// </summary>
        public int TotalAttempts { get; set; }
        /// <summary>
        /// Delegate for callbacks when the statistics are updated.
        /// </summary>
        public delegate void OnStatisticsChangeCallback();
        /// <summary>
        /// Invoked when the statistics about these sessions change.
        /// </summary>
        public event OnStatisticsChangeCallback OnStatisticsChange;
        // Data that can contribute to the statistics.
        private SessionGroupData Data { get; set; }
        // How the level is divided. These ranges must not overlap.
        private List<Range> Dividing { get; set; }
        // Criteria for selecting sessions to compute statistics about.
        private SessionFilterViewModel Filter { get; set; }

        const double MIN_PERCENT = 0;
        const double MAX_PERCENT = 100;

        public SessionsStatistics(SessionGroupData data, SessionFilterViewModel filter, double defaultPartWidth)
        {
            Data = data;
            Filter = filter;
            Dividing = GetParts(defaultPartWidth);
            Statistics = GetStatistics();
            PlayTime = new TimeSpan(Data.Sessions.Sum(s => s.Duration.Ticks) + Data.PracticeSessions.Sum(s => s.Duration.Ticks));
            TotalAttempts = Data.Sessions.Sum(s => s.Attempts.Count()) + Data.PracticeSessions.Sum(s =>s.Attempts.Count());
            if(Filter != null) {
                Filter.OnFilterChanges += UpdateStatistics;
            }
        }

        public SessionsStatistics(SessionGroupData data, double defaultPartWidth) : this(data, null, defaultPartWidth)
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
            var counting = new SortedList<Range, LevelPartStatistics>();
            foreach (var range in Dividing)
            {
                counting.Add(range, new LevelPartStatistics(range));
            }

            GetNormalModeLocalStatistics(ref counting, Data.Sessions);
            if(Filter == null || Filter.ShowPractice == true)
            {
                GetPracticeModeLocalStatistics(ref counting, Data.PracticeSessions);
            }
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

        // Returns the normal mode statistics
        private void GetNormalModeLocalStatistics(ref SortedList<Range, LevelPartStatistics> counting, List<Session> allSessions)
        {
            // Sessions matching the filter
            var sessions = Filter == null ? allSessions : allSessions.Where(s => Filter.Matches(s)).ToList();
            // All attempts of valid sessions
            var attempts = sessions.SelectMany(s => s.Attempts).ToList();

            SetDeathCount(attempts, ref counting);
            SetStartCount(sessions, ref counting);
        }

        // Set the number of death for each part in the SortedList
        private void SetDeathCount(List<Attempt> attempts, ref SortedList<Range, LevelPartStatistics> counting)
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
        private void SetStartCount(List<Session> sessions, ref SortedList<Range, LevelPartStatistics> counting)
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

        // Returns the practice mode statistics
        private List<LevelPartStatistics> GetPracticeModeLocalStatistics(ref SortedList<Range, LevelPartStatistics> counting, List<PracticeSession> practiceSessions)
        {
            var attempts = practiceSessions.SelectMany(s => s.Attempts).ToList();
            SetPracticeLocalCounts(ref counting, attempts);
            return counting.Select(element => element.Value).ToList();
        }

        // Sets the practice local reach count and death count
        private void SetPracticeLocalCounts(ref SortedList<Range, LevelPartStatistics> counting, List<PracticeAttempt> attempts)
        {
            foreach (var a in attempts)
            {
                var spawnRange = Dividing.Find(r => r.Contains(a.StartPercent));
                var deathRange = Dividing.Find(r => r.Contains(a.EndPercent));

                if (counting.TryGetValue(spawnRange, out var attemptStartPartStats))
                {
                    attemptStartPartStats.ReachCount++;
                }

                if (counting.TryGetValue(deathRange, out var attemptDeathPartStats))
                {
                    attemptDeathPartStats.DeathCount++;
                }
            }
        }

        // Updates statistics and notify the change 
        private void UpdateStatistics()
        {
            var sessions = Filter == null ? Data.Sessions : Data.Sessions.Where(s => Filter.Matches(s)).ToList();
            Statistics = GetStatistics();
            PlayTime = new TimeSpan(sessions.Sum(s => s.Duration.Ticks));
            TotalAttempts = sessions.Sum(s => s.Attempts.Count());
            OnStatisticsChange?.Invoke();
        }
    }
}
