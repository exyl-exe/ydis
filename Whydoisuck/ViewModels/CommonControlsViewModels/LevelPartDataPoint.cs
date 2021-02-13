using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.ViewModels.DataStructures;

namespace Ydis.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// Model for on point in the statistics graph.
    /// </summary>
    public class LevelPartDataPoint : IDataPointProvider
    {
        /// <summary>
        /// How the percent range will be displayed
        /// </summary>
        public string PercentRange => string.Format(Properties.Resources.TooltipPercentFormat, Stats.PercentRange.Start, Stats.PercentRange.End);
        /// <summary>
        /// How the reach count will be displayed
        /// </summary>
        public string Reachs => string.Format(Properties.Resources.TooltipReachCountFormat, Stats.ReachCount);
        /// <summary>
        /// How the percent range will be displayed
        /// </summary>
        public string Deaths => string.Format(Properties.Resources.TooltipDeathCountFormat, Stats.DeathCount);
        /// <summary>
        /// How the percent range will be displayed
        /// </summary>
        public string PassRate => string.Format(Properties.Resources.TooltipPassRateFormat, Stats.PassRate);

        // Actual point on the graph
        private DataPoint Point { get; set; }
        // Statistics depicted by the point
        private LevelPartStatistics Stats { get; set; }

        public LevelPartDataPoint(LevelPartStatistics stats)
        {
            Stats = stats;
            Point = new DataPoint(stats.PercentRange.Start, stats.PassRate);
        }

        /// <summary>
        /// Gets the data point for the associated statistics.
        /// </summary>
        /// <returns>The data point</returns>
        public DataPoint GetDataPoint()
        {
            return Point;
        }
    }
}
