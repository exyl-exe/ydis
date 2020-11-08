using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.Navigation
{
    /// <summary>
    /// View model for the navigation panel as a whole.
    /// </summary>
    public class NavigationPanelViewModel
    {
        /// <summary>
        /// View that will be modified depending on how the user navigates
        /// </summary>
        public MainWindowViewModel MainView { get; set; }
        /// <summary>
        /// ViewModel for the search view
        /// </summary>
        public NavigationSearchViewModel SearchView { get; set; }
        /// <summary>
        /// Command to switch the main view to a summary of the current session
        /// </summary>
        public NavigatorCommand GoToCurrentCommand { get; set; }

        public NavigationPanelViewModel(MainWindowViewModel mainWindow)
        {
            MainView = mainWindow;
            GoToCurrentCommand = new NavigatorCommand(mainWindow, new CurrentLevelViewModel(mainWindow.Recorder));
            SearchView = new NavigationSearchViewModel(this, SessionManager.Instance.Groups);
            SessionManager.Instance.OnGroupUpdated += SearchView.UpdateGroup;
        }
    }
}
