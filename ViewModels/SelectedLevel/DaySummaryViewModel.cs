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
        private readonly SortedList<TimeSpan, Session> _sessions;
        public List<Session> Sessions { get => _sessions.Values.Reverse().ToList();}
        public DateTime Day { get; set; }

        public DaySummaryViewModel(DateTime day)
        {
            _sessions = new SortedList<TimeSpan, Session>();
            Day = day;
        }

        public void AddSession(Session session)
        {
            _sessions.Add(session.StartTime.TimeOfDay, session);
        }
    }
}
