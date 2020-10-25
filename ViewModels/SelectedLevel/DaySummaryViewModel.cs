using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Views.Commands;
using Whydoisuck.Views.SelectedSession;
using Whydoisuck.DataModel;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class DaySummaryViewModel
    {
        public SortedList<TimeSpan, SessionButtonViewModel> Sessions { get; }
        public DateTime Day { get; set; }

        public DaySummaryViewModel(DateTime day)
        {
            Sessions = new SortedList<TimeSpan, SessionButtonViewModel>();
            Day = day;
        }

        public DaySummaryViewModel(DateTime day, List<Session> sessions) : this(day)
        {
            foreach(var s in sessions)
            {
                AddSession(s);
            }
        }

        public void AddSession(Session session)
        {
            Sessions.Add(session.StartTime.TimeOfDay, new SessionButtonViewModel(session));
        }
    }
}
