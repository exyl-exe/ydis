using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.ViewModels;
using Whydoisuck.ViewModels.Current;

namespace Whydoisuck.Views.Commands
{
    class NavigatorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private MainWindowViewModel viewModel;

        public NavigatorCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            viewModel.ReplaceMainView(new CurrentLevelViewModel() );
        }
    }
}
