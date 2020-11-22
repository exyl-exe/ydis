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
    /// <summary>
    /// Command that switches a part of a view
    /// </summary>
    public class NavigatorCommand : ICommand
    {
        /// <summary>
        /// Invoked when the command is enabled/disabled
        /// </summary>
        public event EventHandler CanExecuteChanged;
        // The view model for the view which can have one of its parts switched
        private IReplaceableViewViewModel ViewModel { get; set; }
        // The new part of the view
        private BaseViewModel ViewToNavigateTo { get; set; }

        public NavigatorCommand(IReplaceableViewViewModel viewModel, BaseViewModel newView)
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
