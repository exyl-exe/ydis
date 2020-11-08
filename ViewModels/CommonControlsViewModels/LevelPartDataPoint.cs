using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// Model for on point in the statistics graph.
    /// </summary>
    public class LevelPartDataPoint : IDataPointProvider
    {
        /// <summary>
        /// Tooltip information for this point
        /// </summary>
        public string Tooltip {
            get
            {
                return GetText();
            }
        }

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

        /// <summary>
        /// Gets the tooltip text for the current point.
        /// </summary>
        /// <returns>The text that should be in the tooltip.</returns>
        public string GetText()
        {
            return  $"{Stats.PercentRange.Start}%-{Stats.PercentRange.End}%\n"
                    + $"Reached {Stats.ReachCount} times\n"
                    + $"Died {Stats.DeathCount} times\n"
                    + $"{Stats.PassRate}% pass rate";
        }
    }
}
