using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.UIModel.GraphView;

namespace Whydoisuck.UIModel
{
    class AttemptGraph
    {
        private SessionGroup Group { get; set; }

        public AttemptGraph(SessionGroup group)
        {
            Group = group;
        }

        private List<Session> GetRelevantSessions(SessionFilter filter)
        {
            var sessions = Group.GroupSessions.Where(s => filter.Matches(s)).ToList();
            sessions.Sort(Session.CompareStart);
            return sessions;
        }

        public List<LevelPercentData> GetLevelPercentsData(SessionFilter filter, float rangeWidth)
        {
            if (Group == null) return null;

            var res = new List<LevelPercentData>();

            var sessions = GetRelevantSessions(filter);
            var attempts = sessions.SelectMany(s => s.GetSessionAttempts()).ToList();
 
            var attDictionary = GetAttemptRangeList(attempts, rangeWidth);
            var sessionIndex = 0;
            var reachCount = 0;

            for (var i = 0; i < attDictionary.Count; i++)
            {
                var attemptsOfGroup = attDictionary[i].Attempts;
                var range = attDictionary[i].Range;
                var deathCount = attemptsOfGroup.Count;

                //Update reach count
                while (sessionIndex < sessions.Count && (range.GreaterEquals(sessions[sessionIndex].StartPercent)))
                {
                    reachCount += sessions[sessionIndex].Attempts.Count;
                    sessionIndex++;
                }
                var currentPercentData = new LevelPercentData(range, reachCount, deathCount);
                res.Add(currentPercentData);

                reachCount -= deathCount;
            }
            res.Sort((p1, p2) => p1.Compare(p2));
            return res;
        }

        private List<RangeAttemptList> GetAttemptRangeList(List<SessionAttempt> attempts, float rangeWidth)
        {
            attempts.Sort((a,a2)=>a.Attempt.Compare(a2.Attempt));
            var res = new List<RangeAttemptList>();
            RangeAttemptList currentRange = null;
            foreach (var a in attempts)
            {
                if (currentRange == null || !currentRange.Range.Contains(a.Attempt.EndPercent))
                {
                    currentRange = new RangeAttemptList(GetRange(a.Attempt.EndPercent, rangeWidth));
                    res.Add(currentRange);
                }
                currentRange.Attempts.Add(a);
            }
            return res;
        }

        private Range GetRange(float value, float rangeWidth)
        {
            var start = ((int)(value / rangeWidth)) * rangeWidth;
            return new Range(start, start + rangeWidth);
        }
    }
}
