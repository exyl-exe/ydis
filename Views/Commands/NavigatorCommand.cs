using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.ViewModels;
using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.ViewModels.SelectedLevel;

namespace Whydoisuck.Views.Commands
{
    public class NavigatorCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ReplaceableViewViewModel ViewModel { get; set; }
        private BaseViewModel ViewToNavigateTo { get; set; }

        public NavigatorCommand(ReplaceableViewViewModel viewModel, BaseViewModel newView)
        {
            this.ViewModel = viewModel;
            this.ViewToNavigateTo = newView;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.ReplaceView(ViewToNavigateTo);
        }
    }
}
