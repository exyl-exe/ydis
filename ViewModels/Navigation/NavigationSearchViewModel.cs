using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;
using Whydoisuck.Views.NavigationPanel;

namespace Whydoisuck.ViewModels.Navigation
{
    public class NavigationSearchViewModel : BaseViewModel
    {
        public NavigationPanelViewModel ParentNavigationPanel { get; set; }
        public List<NavigationSearchResultViewModel> AllResults { get; set; }
        public List<NavigationSearchResultViewModel> SearchResults { get; set; }

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


        public NavigationSearchViewModel(NavigationPanelViewModel ParentNavigationPanel, List<SessionGroup> groups)
        {
            this.ParentNavigationPanel = ParentNavigationPanel;
            _search = "";
            AllResults = groups.Select(
                g => new NavigationSearchResultViewModel(g, ParentNavigationPanel.MainView, new SelectedLevelViewModel(g))
                ).ToList();
            UpdateSearchResults();
        }

        public void UpdateGroup(SessionGroup group)
        {
            var existingResult = AllResults.Find(res => res.Group.Equals(group));
            if(existingResult == null)
            {
                var newGroup = new NavigationSearchResultViewModel(group, ParentNavigationPanel.MainView, new SelectedLevelViewModel(group));
                AllResults.Add(newGroup);
            } else
            {
                existingResult.Update();
            }
            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            SearchResults = AllResults.Where(result => result.Group.GroupName.ToLower().Trim().StartsWith(_search.ToLower().Trim())).ToList();
            OnPropertyChanged(nameof(SearchResults));
        }
    }
}
