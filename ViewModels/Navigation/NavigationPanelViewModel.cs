using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.ViewModels.SelectedLevel;

namespace Whydoisuck.ViewModels.Navigation
{
    public class NavigationPanelViewModel
    {
        public MainWindowViewModel MainView { get; set; }
        public NavigationSearchViewModel SearchView { get; set; }

        public NavigationPanelViewModel(MainWindowViewModel mainWindow)
        {
            MainView = mainWindow;
            SearchView = new NavigationSearchViewModel(this);
        }
    }
}
