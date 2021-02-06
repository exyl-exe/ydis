using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab
{
    /// <summary>
    /// View model for the view with a list of summaries per day of a group.
    /// </summary>
    public class SessionsSummariesViewModel : BaseViewModel
    {
        /// <summary>
        /// List of view models for each day, ordered by descending date.
        /// </summary>
        public List<DaySummaryViewModel> SortedSummaries => Summaries.Values.Reverse().ToList();

        //Parent view, needed to be able to switch the view.
        private SessionsTabMainViewModel Parent { get; set; }
        //List of summaries of the group.
        private SortedList<DateTime, DaySummaryViewModel> Summaries { get; }
        //Group that is summarized.
        private SessionGroup Group { get; set; }

        public SessionsSummariesViewModel(SessionsTabMainViewModel parent, SessionGroup g)
        {
            Parent = parent;
            Group = g;
            Summaries = CreateSummaries();
        }

        //Creates summaries for the group.
        private SortedList<DateTime, DaySummaryViewModel> CreateSummaries()
        {
            var res = new SortedList<DateTime, DaySummaryViewModel>();
            foreach (var session in Group.GroupData.Sessions)
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
