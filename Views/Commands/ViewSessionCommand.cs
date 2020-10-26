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
    public class ViewSessionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Session Session { get; set; }
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
