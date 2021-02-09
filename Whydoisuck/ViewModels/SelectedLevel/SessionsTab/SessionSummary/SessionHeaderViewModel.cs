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
        public string StartingTime => string.Format(Properties.Resources.SessionHeaderStartTimeFormat, Session.StartTime);

        // Session summarized.
        private ISession Session { get; set; }

        public SessionHeaderViewModel(ISession s)
        {
            Session = s;
        }
    }
}
