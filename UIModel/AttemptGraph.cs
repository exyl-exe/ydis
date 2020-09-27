using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.UIModel.DataStructures;

namespace Whydoisuck.UIModel
{
    class AttemptGraph
    {
        private GroupDisplayer Group { get; set; }

        public AttemptGraph(GroupDisplayer group, SessionFilter filter)
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
                var attemptsOfGroup = attDictionary.At(i);
                var range = GetRange(attemptsOfGroup[0].Attempt.EndPercent, rangeWidth);
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
