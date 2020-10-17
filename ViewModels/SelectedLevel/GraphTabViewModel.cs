using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ModelAgain;
using Whydoisuck.ViewModels.CommonViewModels;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class GraphTabViewModel
    {
        private SessionsStatistics GroupStats { get; set; }
        public LevelDataGridViewModel DataGrid { get; set; }
        public LevelGraphViewModel Graph { get; set; }

        public GraphTabViewModel(SessionGroup g)
        {
            GroupStats = new SessionsStatistics(g.GroupSessions, 1f);
            DataGrid = new LevelDataGridViewModel(GroupStats.Statistics);
            Graph = new LevelGraphViewModel(GroupStats.Statistics);
        }
    }
}
