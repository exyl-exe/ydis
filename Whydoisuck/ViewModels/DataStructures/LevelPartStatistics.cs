using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.ViewModels.DataStructures
{
    /// <summary>
    /// Statistics about a specific part of a level
    /// </summary>
    public class LevelPartStatistics
    {
        /// <summary>
        /// Percents included in the part.
        /// </summary>
        public Range PercentRange { get; set; }
        /// <summary>
        /// How many times the part was reached.
        /// </summary>
        public int ReachCount { get; set; }
        /// <summary>
        /// How many times the player died at this part.
        /// </summary>
        public int DeathCount { get; set; }
        /// <summary>
        /// How consistent the player is on this this part.
        /// </summary>
        public double PassRate
        {
            get
            {
                if (ReachCount == 0) return 0;
                if (PercentRange.Start >= 100) return 100;
                return 100 * (1 - (double)DeathCount / (double)ReachCount);
            }
        }

        public LevelPartStatistics(Range range)
        {
            PercentRange = range;
            ReachCount = 0;
            DeathCount = 0;
        }
    }
}
