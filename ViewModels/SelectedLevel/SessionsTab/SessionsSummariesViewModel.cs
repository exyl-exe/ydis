using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab
{
    public class SessionsSummariesViewModel : BaseViewModel
    {
        private SessionGroup Group { get; set; }
        private SortedList<DateTime, DaySummaryViewModel> Summaries { get; }
        public List<DaySummaryViewModel> SortedSummaries => Summaries.Values.Reverse().ToList();

        private SessionsTabMainViewModel Parent { get; set; }

        public SessionsSummariesViewModel(SessionsTabMainViewModel parent, SessionGroup g)
        {
            Parent = parent;
            Group = g;
            Summaries = CreateSummaries();
        }

        private SortedList<DateTime, DaySummaryViewModel> CreateSummaries()
        {
            var res = new SortedList<DateTime, DaySummaryViewModel>();
            foreach (var session in Group.GroupSessions)
            {
                if (res.TryGetValue(session.StartTime.Date, out var daySummary))
                {
                    daySummary.AddSession(session);
                }
                else
                {
                    var newSummary = new DaySummaryViewModel(Parent, session.StartTime.Date);
                    res.Add(session.StartTime.Date, newSummary);
                    newSummary.AddSession(session);
                }
            }
            return res;
        }
    }
}
