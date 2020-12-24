using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Properties;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.SelectedLevel.GraphTab
{
    /// <summary>
    /// View model for the statistics tab.
    /// </summary>
    public class GraphTabMainViewModel
    {
        /// <summary>
        /// DataGrid of the view, with stats about each % of the level.
        /// </summary>
        public LevelDataGridViewModel DataGrid { get; set; }
        /// <summary>
        /// Graph of the view, with pass rate per %.
        /// </summary>
        public LevelGraphViewModel Graph { get; set; }
        /// <summary>
        /// View model for the filter panel.
        /// </summary>
        public SessionFilterViewModel Filter { get; set; }
        // Statistics of the group
        private SessionsStatistics GroupStats { get; set; }

        public GraphTabMainViewModel(SessionGroup g)
        {
            Filter = new SessionFilterViewModel();
            GroupStats = new SessionsStatistics(g.GroupSessions, Filter, WDISSettings.DefaultPartWidth);
            DataGrid = new LevelDataGridViewModel(GroupStats);
            Graph = new LevelGraphViewModel(GroupStats, Resources.GraphTitleOverall);
        }
    }
}
