using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<Range> Dividing { get; set; }
        public List<LevelPartStatistics> Statistics { get; }

        public SessionsStatistics(List<Session> sessions, float defaultPartWidth)
        {
            Sessions = sessions;
            Dividing = GetParts(defaultPartWidth);
            var sw = Stopwatch.StartNew();
            Statistics = GetStatistics();
        }

        public List<Range> GetParts(float partWidth)
        {
            var res = new List<Range>();
            var currentPartStart = 0f;
            while(currentPartStart < 100f+partWidth)
            {
                res.Add(new Range(currentPartStart, currentPartStart + partWidth));
                currentPartStart += partWidth;
            }
            return res;
        }

        public List<LevelPartStatistics> GetStatistics()
        {
            var res = new List<LevelPartStatistics>();
            var attempts = Sessions.SelectMany(s => s.Attempts).ToList();
            var counting = new SortedList<Range, LevelPartStatistics>();

            foreach(var range in Dividing)
            {
                counting.Add(range, new LevelPartStatistics(range, 0, 0));
            }

            foreach(var attempt in attempts)
            {
                var deathRange = Dividing.Find(r => r.Contains(attempt.EndPercent));
                if(counting.TryGetValue(deathRange, out var attemptDeathPartStats))
                {
                    attemptDeathPartStats.DeathCount++;
                }
            }

            foreach(var session in Sessions)
            {
                var spawnRange = Dividing.Find(r => r.Contains(session.StartPercent));
                if (counting.TryGetValue(spawnRange, out var attemptStartPartStats))
                {
                    attemptStartPartStats.ReachCount += session.Attempts.Count;
                }
            }

            int totalReach = 0;
            foreach (var part in counting.Values)
            {
                if(part.DeathCount > 0)
                {
                    res.Add(part);
                }

                totalReach += part.ReachCount;
                part.ReachCount = totalReach;
                totalReach -= part.DeathCount;
            }

            return res;
        }
    }
}
