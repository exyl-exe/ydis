using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.DataStructures;
using Whydoisuck.ViewModels.SessionSummary;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab.SessionSummary
{
    public class SessionViewModel : BaseViewModel, ReplaceableViewViewModel
    {
        private Session Session { get; }
        public SessionHeaderViewModel Header { get; }
        public LevelGraphViewModel Graph { get; }
        public GoBackCommand GoBack { get; }
        public BaseViewModel CurrentView { get; set; }
        public NavigatorCommand SwitchCommand { get; set; }
        private LevelDataGridViewModel Datagrid { get; }
        private AttemptListViewModel AttemptList { get; }
        private NavigatorCommand AttemptsSummaryCommand { get; }
        private NavigatorCommand AttemptsDetailsCommand { get; }
        private bool ShowingDetails { get; set; }

        public SessionViewModel(SessionsTabMainViewModel parent, Session s)
        {
            Session = s;
            GoBack = new GoBackCommand(parent);
            Header = new SessionHeaderViewModel(Session);
            var l = new List<Session> { Session };
            var stats = new SessionsStatistics(l, 1f);
            Graph = new LevelGraphViewModel(stats);

            Datagrid = new LevelDataGridViewModel(stats);
            AttemptList = new AttemptListViewModel(s.Attempts);
            AttemptsSummaryCommand = new NavigatorCommand(this, Datagrid);
            AttemptsDetailsCommand = new NavigatorCommand(this, AttemptList);

            ShowingDetails = false;
            CurrentView = Datagrid;
            SwitchCommand = AttemptsDetailsCommand;
        }

        public void ReplaceView(BaseViewModel m)
        {
            CurrentView = m;
            SwitchCommand = ShowingDetails ? AttemptsDetailsCommand : AttemptsSummaryCommand;
            ShowingDetails = !ShowingDetails;
            OnPropertyChanged(nameof(CurrentView));
            OnPropertyChanged(nameof(SwitchCommand));
        }
    }
}
