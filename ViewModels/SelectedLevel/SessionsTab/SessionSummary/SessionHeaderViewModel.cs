using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.SessionSummary
{
    /// <summary>
    /// View model for the header of a session summary
    /// </summary>
    public class SessionHeaderViewModel : BaseViewModel
    {
        /// <summary>
        /// How the name of the level is displayed
        /// </summary>
        public string LevelName => Session.Level.Name;
        /// <summary>
        /// How the starting time of the session
        /// </summary>
        public string StartingTime => string.Format(Properties.Resources.SessionStartTimeFormat, Session.StartTime.TimeOfDay);
        /// <summary>
        /// How the start percent of the session
        /// </summary>
        public string StartPercent => string.Format(Properties.Resources.SessionStartPercentFormat, Session.StartPercent);
        /// <summary>
        /// How the attempt count is displayed
        /// </summary>
        public string AttemptCount => string.Format(Properties.Resources.SessionAttemptCountFormat, Session.Attempts.Count);

        // Session summarized.
        private Session Session { get; set; }

        public SessionHeaderViewModel(Session s)
        {
            Session = s;
        }
    }
}
