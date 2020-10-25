using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Views.Commands;
using Whydoisuck.Views.SelectedSession;
using Whydoisuck.DataModel;
using System.Windows;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class DaySummaryViewModel : BaseViewModel
    {
        public Visibility SummaryVisibility => Sessions.Values.Where(svm => svm.Visible).Count()>0?
                                        Visibility.Visible
                                        :Visibility.Collapsed;
        public SortedList<TimeSpan, SessionButtonViewModel> Sessions { get; }
        public DateTime Day { get; set; }

        private SessionsTabViewModel Parent { get; set; }

        public DaySummaryViewModel(SessionsTabViewModel parent, DateTime day)
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
