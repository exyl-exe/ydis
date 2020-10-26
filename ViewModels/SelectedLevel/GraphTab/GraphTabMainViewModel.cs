using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.SelectedLevel.GraphTab
{
    public class GraphTabMainViewModel
    {
        private SessionsStatistics GroupStats { get; set; }
        public LevelDataGridViewModel DataGrid { get; set; }
        public LevelGraphViewModel Graph { get; set; }

        public GraphTabMainViewModel(SessionGroup g)
        {
            GroupStats = new SessionsStatistics(g.GroupSessions, 1f);
            DataGrid = new LevelDataGridViewModel(GroupStats.Statistics);
            Graph = new LevelGraphViewModel(GroupStats.Statistics);
        }
    }
}
