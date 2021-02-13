using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ydis.Model.DataStructures;
using Ydis.Properties;
using Ydis.Views.Commands;

namespace Ydis.ViewModels.SelectedLevel.SessionsTab
{
    /// <summary>
    /// View model for buttons that open a detailled summary of a session
    /// </summary>
    public class SessionButtonViewModel : BaseViewModel, ISessionButtonViewModel
    {
        /// <summary>
        /// Session that this button will open a summary of
        /// </summary>
        public ISession Session { get; private set; }
        /// <summary>
        /// Command that opens the summary of a session
        /// </summary>
        public ICommand ViewSessionCommand { get; set; }

        public string CopyName => Session.Level.Name;
        public string Time => string.Format(Resources.SessionButtonTimeFormat, Session.StartTime.TimeOfDay);
        public string Start => string.Format(Resources.SessionButtonStartFormat, ((Session)Session).StartPercent);
        public string AttemptCount => string.Format(Resources.SessionButtonCountFormat, ((Session)Session).Attempts.Count);

        public SessionButtonViewModel(SessionsTabMainViewModel parent, Session session)
        {
            Session = session;
            ViewSessionCommand = new ViewSessionCommand(parent, session);
        }
    }
}
