using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.ViewModels;
using Whydoisuck.ViewModels.Current;
using Whydoisuck.ViewModels.SelectedLevel;

namespace Whydoisuck.Views.Commands
{
    public class NavigatorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainWindowViewModel viewModel;
        private BaseViewModel viewToNavigateTo;

        public NavigatorCommand(MainWindowViewModel viewModel, BaseViewModel newView)
        {
            this.viewModel = viewModel;
            this.viewToNavigateTo = newView;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.ReplaceMainView(viewToNavigateTo);
        }
    }
}
