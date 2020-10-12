using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.Navigation
{
    public class NavigationSearchViewModel : BaseViewModel
    {
        public NavigationPanelViewModel NavigationPanel { get; set; }

        public List<object> SearchResults { get; set; }

        public NavigationSearchViewModel(NavigationPanelViewModel NavigationPanel)
        {
            SearchResults = new List<object>();
            this.NavigationPanel = NavigationPanel;
            foreach (var l in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                SearchResults.Add(new { UpdateCommand = new NavigatorCommand(NavigationPanel.MainView, new SelectedLevelViewModel()),
                                        text = l.ToString()});
            }
        }
    }
}
