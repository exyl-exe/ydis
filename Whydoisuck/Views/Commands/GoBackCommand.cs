using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab;

namespace Whydoisuck.Views.Commands
{
    /// <summary>
    /// Command to stop displaying information about a session and go back to the session list.
    /// </summary>
    public class GoBackCommand : ICommand
    {
        /// <summary>
        /// Invoked when the command is enabled/disabled
        /// </summary>
        public event EventHandler CanExecuteChanged;

        // The parent view, containing the view with a session details
        private SessionsTabMainViewModel SessionsTab { get; set; }

        public GoBackCommand(SessionsTabMainViewModel parent)
        {
            SessionsTab = parent;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SessionsTab.PopView();
        }
    }
}
