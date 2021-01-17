using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.ViewModels;
using Whydoisuck.ViewModels.FolderManagement;
using Whydoisuck.ViewModels.Navigation;
using Whydoisuck.Views.FolderManagement;

namespace Whydoisuck.Views.Commands
{
    /// <summary>
    /// Command for switching from normal view to folder selection view
    /// </summary>
    public class StartFolderSelectionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private MainWindowViewModel MainWindow { get; set; }
        private FolderManagementViewModel FolderManagement { get; set; }
        private NavigationPanelViewModel NavPanel { get; set; }
        private FolderSelectorViewModel Selector { get; set; }

        public StartFolderSelectionCommand(MainWindowViewModel mainWindow,
                                           FolderManagementViewModel folderManagement,
                                           NavigationPanelViewModel navPanel,
                                           FolderSelectorViewModel selector)
        {
            MainWindow = mainWindow;
            FolderManagement = folderManagement;
            NavPanel = navPanel;
            Selector = selector;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MainWindow.ReplaceView(FolderManagement);
            NavPanel.ReplaceView(Selector);
        }
    }
}
