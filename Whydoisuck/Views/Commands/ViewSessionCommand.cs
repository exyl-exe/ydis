using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ydis.Model.DataStructures;
using Ydis.ViewModels.SelectedLevel.SessionsTab;
using Ydis.ViewModels.SelectedLevel.SessionsTab.SessionSummary;
using Ydis.Views.SelectedLevel;

namespace Ydis.Views.Commands
{
    /// <summary>
    /// Command opening the view displaying information about one specific session
    /// </summary>
    public class ViewSessionCommand : ICommand
    {
        /// <summary>
        /// Invoked when the command is enabled/disabled
        /// </summary>
        public event EventHandler CanExecuteChanged;
        // The session that will be opened
        private ISession Session { get; set; }
        // The parent view, where the session details will be displayed
        private SessionsTabMainViewModel SessionsTab { get; set; }

        public ViewSessionCommand(SessionsTabMainViewModel parent, ISession s)
        {
            SessionsTab = parent;
            Session = s;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SessionsTab.PushView(new SessionViewModel(SessionsTab, Session));
        }
    }
}
