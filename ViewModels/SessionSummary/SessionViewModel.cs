using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ModelAgain;
using Whydoisuck.ViewModels.CommonViewModels;

namespace Whydoisuck.ViewModels.SessionSummary
{
    public class SessionViewModel : BaseViewModel
    {
        private Session Session { get; set; }
        public SessionHeaderViewModel Header { get; set; }
        public LevelGraphViewModel Graph { get; set; }
        public LevelDataGridViewModel Datagrid { get; set; }

        public SessionViewModel(Session s)
        {
            Session = s;
            var l = new List<Session> { Session };
            var stats = new SessionsStatistics(l, 1f);
            Header = new SessionHeaderViewModel(Session);
            Graph = new LevelGraphViewModel(stats.Statistics);
            Datagrid = new LevelDataGridViewModel(stats.Statistics);
        }
    }
}
