using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab.SessionSummary;
using Whydoisuck.Views.SelectedLevel;

namespace Whydoisuck.Views.Commands
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
        private Session Session { get; set; }
        // The parent view, where the session details will be displayed
        private SessionsTabMainViewModel SessionsTab { get; set; }

        public ViewSessionCommand(SessionsTabMainViewModel parent, Session s)
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
