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
    public class NavigationPanelViewModel
    {
        public MainWindowViewModel MainView { get; set; }
        public NavigationSearchViewModel SearchView { get; set; }
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
