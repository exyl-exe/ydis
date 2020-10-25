using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ViewModels.CommonViewModels;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class SessionsTabViewModel : BaseViewModel
    {
        private SessionGroup Group { get; set; }
        private SortedList<DateTime, DaySummaryViewModel> AllSummaries { get; set; }
        public SortedList<DateTime, DaySummaryViewModel> Summaries => GetFilteredSummaries();

        public PercentRangeSliderViewModel SelectRange { get; set; }

        public SessionsTabViewModel(SessionGroup g)
        {
            Group = g;
            SelectRange = new PercentRangeSliderViewModel();
            SelectRange.OnRangeChanged += Update;
            AllSummaries = CreateSummaries();
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

        private void Update(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Summaries));
        }

        private SortedList<DateTime, DaySummaryViewModel> GetFilteredSummaries()
        {
            var res = new SortedList<DateTime, DaySummaryViewModel>();
            foreach(var summary in AllSummaries)
            {
                var sessions = summary.Value.Sessions.Where(s => SelectRange.Range.Contains(s.StartPercent)).ToList();
                if(sessions.Count > 0)
                {
                    res.Add(summary.Key, new DaySummaryViewModel(summary.Key, sessions));
                }
            }
            return res;
        }
    }
}
