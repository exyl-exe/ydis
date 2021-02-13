using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.ViewModels.DataStructures;

namespace Ydis.ViewModels.CommonControlsViewModels
{
    /// <summary>
    /// View model for one item in a level datagrid.
    /// </summary>
    public class LevelDatagridItemViewModel
    {
        /// <summary>
        /// Start of the percent range which stats are displayed
        /// </summary>
        public double Start => stats.PercentRange.Start;
        /// <summary>
        /// How many times the part was reached
        /// </summary>
        public int ReachCount => stats.ReachCount;
        /// <summary>
        /// How many times the player died at this part
        /// </summary>
        public int DeathCount => stats.DeathCount;
        /// <summary>
        /// How much the player successfully pass this part
        /// </summary>
        public double PassRate => stats.PassRate;

        // Stats that will be shown
        private LevelPartStatistics stats;

        public LevelDatagridItemViewModel(LevelPartStatistics stats)
        {
            this.stats = stats;
        }
    }
}
