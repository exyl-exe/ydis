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
    /// <summary>
    /// View model for the search in the navigation panel
    /// </summary>
    public class NavigationSearchViewModel : BaseViewModel
    {
        /// <summary>
        /// Navigation panel, used to be able to switch views.
        /// </summary>
        public NavigationPanelViewModel ParentNavigationPanel { get; set; }
        /// <summary>
        /// All possible results for the search
        /// </summary>
        public List<NavigationSearchResultViewModel> AllResults { get; set; }
        /// <summary>
        /// Results matching the search
        /// </summary>
        public List<NavigationSearchResultViewModel> SearchResults { get; set; }

        private string _search;
        /// <summary>
        /// What's typed in the search bar.
        /// </summary>
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

        /// <summary>
        /// Updates the search result corresponding to a given group.
        /// </summary>
        /// <param name="group">Group to update</param>
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

        // Updates matching search result and notifies the change.
        private void UpdateSearchResults()
        {
            SearchResults = AllResults.Where(result => result.ResultText.ToLower().Trim().StartsWith(_search.ToLower().Trim()))
                                      .OrderByDescending(res => res.Group.LastPlayedTime)
                                      .ToList();
            OnPropertyChanged(nameof(SearchResults));
        }
    }
}
