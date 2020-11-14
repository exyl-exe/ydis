using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Views.Commands;
using Whydoisuck.Views.SelectedLevel;
using System.Windows;
using Whydoisuck.Model.DataStructures;
using System.Security.Cryptography;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab
{
    /// <summary>
    /// View model for a the summary of a single day about a group.
    /// </summary>
    public class DaySummaryViewModel : BaseViewModel
    {
        /// <summary>
        /// Wether this summary should be displayed or not. It is not displayed
        /// if none of the sessions of that day is visible.
        /// </summary>
        public Visibility SummaryVisibility => Sessions.Where(svm => svm.Visible).Count() > 0 ?
                                        Visibility.Visible
                                        : Visibility.Collapsed;

        public string HeaderText => String.Format(Properties.Resources.DaySummaryHeaderFormat, Day);

        /// Day that is summarized.s
        private DateTime Day { get; set; }

        /// <summary>
        /// Sessions that took place that day, ordered by descending date.
        /// </summary>
        public List<SessionButtonViewModel> SortedSessions => Sessions.OrderByDescending(s => s.Session.StartTime).ToList();

        private List<SessionButtonViewModel> Sessions { get; }

        //Parent view, used to be able to switch a part of the view
        private SessionsTabMainViewModel Parent { get; set; }

        public DaySummaryViewModel(SessionsTabMainViewModel parent, DateTime day)
        {
            Sessions = new List<SessionButtonViewModel>();
            Day = day;
            Parent = parent;
        }

        /// <summary>
        /// Adds a session to that day.
        /// </summary>
        /// <param name="session"></param>
        public void AddSession(Session session)
        {
            Sessions.Add(new SessionButtonViewModel(Parent, session));
        }

        /// <summary>
        /// Notifies a change about the visibility of the summary
        /// </summary>
        public void UpdateVisibility()
        {
            OnPropertyChanged(nameof(SummaryVisibility));
        }
    }
}
