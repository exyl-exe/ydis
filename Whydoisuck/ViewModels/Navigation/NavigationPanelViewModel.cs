using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Properties;
using Whydoisuck.ViewModels.AppSettings;
using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.ViewModels.FolderManagement;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;
using Whydoisuck.Views.CurrentLevel;

namespace Whydoisuck.ViewModels.Navigation
{
    /// <summary>
    /// View model for the navigation panel as a whole.
    /// </summary>
    public class NavigationPanelViewModel : BaseViewModel, IReplaceableViewViewModel
    {
        /// <summary>
        /// View that will be modified depending on how the user navigates
        /// </summary>
        public MainWindowViewModel MainView { get; set; }
        /// <summary>
        /// View model for the folder search area
        /// </summary>
        public BaseViewModel CurrentSearchView { get; set; }

        /// <summary>
        /// Command to switch the main view to a summary of the current session
        /// </summary>
        public NavigatorCommand GoToCurrentCommand { get; set; }
        /// <summary>
        /// Label on the button that opens folder management
        /// </summary>
        public string ManageGroupsText => Resources.ManageFoldersButton;
        /// <summary>
        /// Command to execute when clicking the folder management button
        /// </summary>
        public NavigatorCommand FolderManagementButtonCommand { get; private set; }
        // Command to switch to the folder management view
        private NavigatorCommand FolderManagementCommand { get; set; }
        /// <summary>
        /// Label on the button that opens settings
        /// </summary>
        public string SettingsButtonText => Resources.SettingsButton;
        /// <summary>
        /// Command to switch the main view to the application settings
        /// </summary>
        public NavigatorCommand SettingsCommand { get; set; }

        // ViewModel for the search view
        private NavigationSearchViewModel SearchView { get; set; }

        // ViewModel for the selector view
        private FolderSelectorViewModel FolderSelectorView { get; set; }

        public NavigationPanelViewModel(MainWindowViewModel mainWindow, CurrentLevelViewModel currentSession)
        {
            MainView = mainWindow;
            SearchView = new NavigationSearchViewModel(this, SessionManager.Instance.Groups);
            FolderSelectorView = new FolderSelectorViewModel(SessionManager.Instance.Groups);
            CurrentSearchView = SearchView;

            SessionManager.Instance.OnGroupUpdated += SearchView.UpdateGroup;
            SessionManager.Instance.OnGroupDeleted += SearchView.DeleteGroup;
            SessionManager.Instance.OnGroupUpdated += FolderSelectorView.UpdateGroup;
            SessionManager.Instance.OnGroupDeleted += FolderSelectorView.DeleteGroup;

            var folderManagementVM = new FolderManagementViewModel(mainWindow, currentSession, FolderSelectorView);
            FolderManagementCommand = new NavigatorCommand(mainWindow, folderManagementVM);

            GoToCurrentCommand = new NavigatorCommand(mainWindow, currentSession);
            FolderManagementButtonCommand = FolderManagementCommand;
            SettingsCommand = new NavigatorCommand(mainWindow, new SettingsViewModel());
        }

        /// <summary>
        /// uses the default view for the group selector
        /// </summary>
        public void SwitchToDefaultView()
        {
            if (CurrentSearchView != SearchView)
            {
                ReplaceView(SearchView);
                FolderManagementButtonCommand = FolderManagementCommand;
                OnPropertyChanged(nameof(FolderManagementButtonCommand));
            }
        }

        /// <summary>
        /// uses the selection view for the group selector
        /// </summary>
        public void SwitchToSelectionView()
        {
            if (CurrentSearchView != FolderSelectorView)
            {
                FolderSelectorView.ResetSelection();
                ReplaceView(FolderSelectorView);
                FolderManagementButtonCommand = GoToCurrentCommand;
                OnPropertyChanged(nameof(FolderManagementButtonCommand));
            }
        }

        public void ReplaceView(BaseViewModel m)
        {
            if(m.Equals(SearchView) || m.Equals(FolderSelectorView))
            CurrentSearchView = m;
            OnPropertyChanged(nameof(CurrentSearchView));
        }
    }
}
