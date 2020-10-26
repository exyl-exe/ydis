﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ModelAgain;
using Whydoisuck.ViewModels.CommonViewModels;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.SessionSummary
{
    public class SessionViewModel : BaseViewModel
    {
        private Session Session { get; }
        public SessionHeaderViewModel Header { get; }
        public LevelGraphViewModel Graph { get; }
        public LevelDataGridViewModel Datagrid { get; }
        public GoBackCommand GoBack { get; }

        public SessionViewModel(SessionsTabViewModel parent, Session s)
        {
            Session = s;
            GoBack = new GoBackCommand(parent);
            Header = new SessionHeaderViewModel(Session);
            var l = new List<Session> { Session };
            var stats = new SessionsStatistics(l, 1f);
            Graph = new LevelGraphViewModel(stats.Statistics);
            Datagrid = new LevelDataGridViewModel(stats.Statistics);
        }
    }
}
