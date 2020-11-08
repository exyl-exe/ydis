using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Views.Commands;
using Whydoisuck.Views.SelectedLevel;
using System.Windows;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab
{
    public class DaySummaryViewModel : BaseViewModel
    {
        public Visibility SummaryVisibility => Sessions.Values.Where(svm => svm.Visible).Count() > 0 ?
                                        Visibility.Visible
                                        : Visibility.Collapsed;
        public DateTime Day { get; set; }

        public List<SessionButtonViewModel> SortedSessions => Sessions.Values.Reverse().ToList();

        private SortedList<TimeSpan, SessionButtonViewModel> Sessions { get; }

        private SessionsTabMainViewModel Parent { get; set; }

        public DaySummaryViewModel(SessionsTabMainViewModel parent, DateTime day)
        {
            Sessions = new SortedList<TimeSpan, SessionButtonViewModel>();
            Day = day;
            Parent = parent;
        }

        public void AddSession(Session session)
        {
            Sessions.Add(session.StartTime.TimeOfDay, new SessionButtonViewModel(Parent, session));
        }

        public void UpdateVisibility()
        {
            OnPropertyChanged(nameof(SummaryVisibility));
        }
    }
}
