using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.ViewModels.Navigation;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.FolderManagement
{
    public class FolderManagementViewModel : BaseViewModel
    {
        /// <summary>
        /// Command to quit folder management view
        /// </summary>
        public NavigatorCommand GoBackCommand { get; set; }

        private MainWindowViewModel MainWindow { get; set; }
        private CurrentLevelViewModel CurrentSession { get; set; }
        private NavigationPanelViewModel NavigationPanelViewModel { get; set; }

        public FolderManagementViewModel(MainWindowViewModel mainWindow, CurrentLevelViewModel currentSession, NavigationPanelViewModel navigationPanelViewModel)
        {
            MainWindow = mainWindow;
            CurrentSession = currentSession;
            NavigationPanelViewModel = navigationPanelViewModel;

            GoBackCommand = new NavigatorCommand(MainWindow, CurrentSession);
        }
    }
}