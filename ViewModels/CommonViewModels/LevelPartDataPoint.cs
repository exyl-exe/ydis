using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.UIModel;

namespace Whydoisuck.ViewModels.CommonViewModels
{
    public class LevelPartDataPoint : IDataPointProvider
    {
        private DataPoint Point { get; set; }
        private LevelPartStatistics Stats { get; set; }
        public string Tooltip {
            get
            {
                return GetText();
            }
        }

        public LevelPartDataPoint(LevelPartStatistics stats)
        {
            Stats = stats;
            Point = new DataPoint(stats.PercentRange.Start, stats.PassRate);
        }

        public DataPoint GetDataPoint()
        {
            return Point;
        }

        public string GetText()
        {
            return  $"{Stats.PercentRange.Start}%-{Stats.PercentRange.End}%\n"
                    + $"Reached {Stats.ReachCount} times\n"
                    + $"Died {Stats.DeathCount} times\n"
                    + $"{Stats.PassRate}% pass rate";
        }
    }
}
