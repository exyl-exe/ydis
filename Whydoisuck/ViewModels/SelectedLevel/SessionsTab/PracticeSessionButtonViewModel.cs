using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Properties;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab
{
    public class PracticeSessionButtonViewModel : BaseViewModel, ISessionButtonViewModel
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
        public string Start => "#Practice";
        public string AttemptCount => string.Format(Resources.SessionButtonCountFormat, ((PracticeSession)Session).Attempts.Count);

        public PracticeSessionButtonViewModel(SessionsTabMainViewModel parent, PracticeSession session)
        {
            Session = session;
            ViewSessionCommand = new ViewSessionCommand(parent, session);
        }
    }
}
