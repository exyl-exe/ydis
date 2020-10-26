using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.ViewModels.SelectedLevel;

namespace Whydoisuck.Views.Commands
{
    public class GoBackCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private SessionsTabViewModel SessionsTab { get; set; }

        public GoBackCommand(SessionsTabViewModel parent)
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
