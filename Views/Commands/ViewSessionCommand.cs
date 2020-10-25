using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.DataModel;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.ViewModels.SessionSummary;
using Whydoisuck.Views.SelectedSession;

namespace Whydoisuck.Views.Commands
{
    class ViewSessionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Session Session { get; set; }
        private SessionsTabViewModel SessionsTab { get; set; }

        public ViewSessionCommand(SessionsTabViewModel parent, Session s)
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
            SessionsTab.PushView(new SessionViewModel(Session));
        }
    }
}
