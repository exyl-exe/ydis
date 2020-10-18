using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class SessionsTabViewModel
    {
        private SessionGroup Group { get; set; }
        public SortedList<DateTime, DaySummaryViewModel> Summaries { get; set; }

        public SessionsTabViewModel(SessionGroup g)
        {
            Group = g;
            Summaries = CreateSummaries();
            Console.WriteLine(Summaries.Count);
        }

        private SortedList<DateTime, DaySummaryViewModel> CreateSummaries()
        {
            var res = new SortedList<DateTime, DaySummaryViewModel>();
            foreach(var session in Group.GroupSessions)
            {
                var startTime = session.StartTime;
                if(res.TryGetValue(startTime.Date, out var daySummary))
                {
                    daySummary.AddSession(session);
                } else
                {
                    var newSummary = new DaySummaryViewModel(startTime.Date);
                    res.Add(startTime.Date, newSummary);
                    newSummary.AddSession(session);
                }
            }
            return res;
        }
    }
}
