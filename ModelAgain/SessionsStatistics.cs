using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.UIModel;
using Whydoisuck.UIModel.GraphView;

namespace Whydoisuck.ModelAgain
{
    public class SessionsStatistics
    {
        private List<Session> Sessions { get; set; }
        private float Width { get; set; }
        public List<LevelPercentData> Statistics { get { return GetLevelPercentsData(Width); } }

        public SessionsStatistics(List<Session> sessions, float rangeWidth)
        {
            Sessions = sessions;
            Width = rangeWidth;
        }

        public List<LevelPercentData> GetLevelPercentsData(float rangeWidth)
        {
            var res = new List<LevelPercentData>();

            var attempts = Sessions.SelectMany(s => s.GetSessionAttempts()).ToList();

            var attDictionary = GetAttemptRangeList(attempts, rangeWidth);
            var sessionIndex = 0;
            var reachCount = 0;

            for (var i = 0; i < attDictionary.Count; i++)
            {
                var attemptsOfGroup = attDictionary[i].Attempts;
                var range = attDictionary[i].Range;
                var deathCount = attemptsOfGroup.Count();

                //Update reach count
                while (sessionIndex < Sessions.Count && (range.GreaterEquals(Sessions[sessionIndex].StartPercent)))
                {
                    reachCount += Sessions[sessionIndex].Attempts.Count;
                    sessionIndex++;
                }
                var currentPercentData = new LevelPercentData(range, reachCount, deathCount);
                res.Add(currentPercentData);

                reachCount -= deathCount;
            }
            res.Sort((p1, p2) => p1.Compare(p2));
            return res;
        }

        private List<RangeOfAttempts> GetAttemptRangeList(List<SessionAttempt> attempts, float rangeWidth)
        {
            attempts.Sort((a, a2) => a.Attempt.Compare(a2.Attempt));
            var res = new List<RangeOfAttempts>();
            RangeOfAttempts currentRange = null;
            foreach (var a in attempts)
            {
                if (currentRange == null || !currentRange.Range.Contains(a.Attempt.EndPercent))
                {
                    currentRange = new RangeOfAttempts(GetRange(a.Attempt.EndPercent, rangeWidth));
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
