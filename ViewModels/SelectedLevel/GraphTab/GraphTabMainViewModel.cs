using System;
using System.Collections.Generic;
using System.Data;
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
        public SessionFilterViewModel Filter { get; set; }

        public GraphTabMainViewModel(SessionGroup g)
        {
            Filter = new SessionFilterViewModel();
            GroupStats = new SessionsStatistics(g.GroupSessions, Filter, 1f);
            DataGrid = new LevelDataGridViewModel(GroupStats);
            Graph = new LevelGraphViewModel(GroupStats);
        }
    }
}
