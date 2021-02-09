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
        public string HeaderText => String.Format(Properties.Resources.DaySummaryHeaderFormat, Day);

        /// Day that is summarized.s
        private DateTime Day { get; set; }

        /// <summary>
        /// Sessions that took place that day, ordered by descending date.
        /// </summary>
        public List<ISessionButtonViewModel> SortedSessions => Sessions.OrderByDescending(s => s.Session.StartTime).ToList();

        private List<ISessionButtonViewModel> Sessions { get; }

        //Parent view, used to be able to switch a part of the view
        private SessionsTabMainViewModel Parent { get; set; }

        public DaySummaryViewModel(SessionsTabMainViewModel parent, DateTime day)
        {
            Sessions = new List<ISessionButtonViewModel>();
            Day = day;
            Parent = parent;
        }

        /// <summary>
        /// Adds a session to that day.
        /// </summary>
        /// <param name="session"></param>
        public void AddSession(ISession session)
        {
            if(session is Session ns) { 
                Sessions.Add(new SessionButtonViewModel(Parent, ns));
            } else if (session is PracticeSession ps)
            {
                Sessions.Add(new PracticeSessionButtonViewModel(Parent, ps));
            }
        }
    }
}
