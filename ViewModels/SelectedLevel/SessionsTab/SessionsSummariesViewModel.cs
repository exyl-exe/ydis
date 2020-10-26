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
        public SortedList<DateTime, DaySummaryViewModel> Summaries { get; }
        public PercentRangeSliderViewModel SelectRange { get; set; }

        private SessionsTabMainViewModel Parent { get; set; }

        public SessionsSummariesViewModel(SessionsTabMainViewModel parent, SessionGroup g)
        {
            Parent = parent;
            Group = g;
            SelectRange = new PercentRangeSliderViewModel();
            SelectRange.OnRangeChanged += Update;
            Summaries = CreateSummaries();
        }

        private SortedList<DateTime, DaySummaryViewModel> CreateSummaries()
        {
            var res = new SortedList<DateTime, DaySummaryViewModel>();
            foreach (var session in Group.GroupSessions)
            {
                var startTime = session.StartTime;
                if (res.TryGetValue(startTime.Date, out var daySummary))
                {
                    daySummary.AddSession(session);
                }
                else
                {
                    var newSummary = new DaySummaryViewModel(Parent, startTime.Date);
                    res.Add(startTime.Date, newSummary);
                    newSummary.AddSession(session);
                }
            }
            return res;
        }

        private void UpdateSummaries()
        {
            foreach (var summary in Summaries.Values)
            {
                foreach (var sessionModel in summary.Sessions.Values)
                {
                    if (SelectRange.Range.Contains(sessionModel.Session.StartPercent))
                    {
                        sessionModel.Visible = true;
                    }
                    else
                    {
                        sessionModel.Visible = false;
                    }
                }
                summary.UpdateVisibility();
            }
            OnPropertyChanged(nameof(Summaries));
        }

        private void Update(object sender, EventArgs e)
        {
            UpdateSummaries();
        }
    }
}
