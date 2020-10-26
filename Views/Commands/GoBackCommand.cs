using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab;

namespace Whydoisuck.Views.Commands
{
    public class GoBackCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

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
