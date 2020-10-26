using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;
using Whydoisuck.Views.NavigationPanel;

namespace Whydoisuck.ViewModels.Navigation
{
    public class NavigationSearchViewModel : BaseViewModel
    {
        private SessionManager Manager { get; set; }
        public NavigationPanelViewModel ParentNavigationPanel { get; set; }
        public List<NavigationSearchResult> AllResults { get; set; }
        public List<NavigationSearchResult> SearchResults { get; set; }

        private string _search;
        public string Search {
            get {
                return _search;
            }
            set { 
                if(_search != value)
                {
                    _search = value;
                    OnPropertyChanged(nameof(Search));
                    UpdateSearchResults();
                }
            }
        }

        public NavigationSearchViewModel(NavigationPanelViewModel ParentNavigationPanel, SessionManager manager)
        {
            this.ParentNavigationPanel = ParentNavigationPanel;
            _search = "";
            Manager = manager;
            var groups = Manager.Groups;
            AllResults = groups.Select(
                g => new NavigationSearchResult(g, new NavigatorCommand(ParentNavigationPanel.MainView, new SelectedLevelViewModel(g)))
                ).ToList();
            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            SearchResults = AllResults.Where(result => result.Group.GroupName.ToLower().Trim().StartsWith(_search.ToLower().Trim())).ToList();
            OnPropertyChanged(nameof(SearchResults));
        }
    }
}
