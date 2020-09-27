using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.UIModel.DataStructures;

namespace Whydoisuck.UIModel
{
    class AttemptGraph
    {
        private GroupDisplayer Group { get; set; }
        public SessionFilter Filter { get; set; }

        public AttemptGraph(GroupDisplayer group, SessionFilter filter)
        {
            Group = group;
            Filter = filter;
        }

        public List<LevelPercentData> GetLevelPercentsData(float rangeWidth)//TODO défintivement un gros bordel
        {
            if (Group == null) return null;

            var sessions = Group.GroupSessions.Where(s => Filter.Matches(s)).ToList();
            var attempts = sessions
                .SelectMany(s => s.Attempts.Select(a => new SessionAttempt() { Attempt = a, Session = s }).ToList())
                .ToList();

            sessions.Sort((s, s2) => (int)((s.StartPercent - s2.StartPercent) / Math.Abs(s.StartPercent - s2.StartPercent)));
            var sessionIndex = 0;

            var attDictionary = GetAttemptRangeList(attempts, rangeWidth);
            var percents = new List<LevelPercentData>();
            var reachCount = 0;
            for (var i = 0; i < attDictionary.Count; i++)
            {
                var attemptsOfGroup = attDictionary.At(i);
                var range = GetRange(attemptsOfGroup[0].Attempt.EndPercent, rangeWidth);

                while (sessionIndex < sessions.Count
                    && (range.Contains(sessions[sessionIndex].StartPercent) || sessions[sessionIndex].StartPercent < range.Start))
                {
                    reachCount += sessions[sessionIndex].Attempts.Count;
                    sessionIndex++;
                }

                var deathCount = attemptsOfGroup.Count;

                var currentPercentData = new LevelPercentData
                {
                    PercentRange = range,
                    ReachCount = reachCount,
                    DeathCount = deathCount,
                };
                percents.Add(currentPercentData);
                reachCount -= deathCount;
            }
            percents.Sort((p1, p2) => p1.Compare(p2));
            return percents;
        }

        private SelectDictionary<SessionAttempt, List<SessionAttempt>> GetAttemptRangeList(List<SessionAttempt> attempts, float rangeWidth)
        {
            var dictionary = new SelectDictionary<SessionAttempt, List<SessionAttempt>>((sa) => GetRange(sa.Attempt.EndPercent, rangeWidth).Start);
            foreach (var a in attempts)
            {
                var attemptList = dictionary.Get(a);
                if (attemptList == null)
                {
                    dictionary.Affect(a, new List<SessionAttempt>() { a });
                }
                else
                {
                    attemptList.Add(a);
                }
            }
            return dictionary;
        }

        private Range GetRange(float value, float rangeWidth)
        {
            var start = ((int)(value / rangeWidth)) * rangeWidth;
            return new Range(start, start + rangeWidth);
        }

    }
}
