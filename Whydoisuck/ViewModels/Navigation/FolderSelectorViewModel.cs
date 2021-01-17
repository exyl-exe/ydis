using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;

namespace Whydoisuck.ViewModels.Navigation
{
    /// <summary>
    /// View model for a list of selectable folders
    /// </summary>
    public class FolderSelectorViewModel : BaseViewModel
    {
        /// <summary>
        /// Search bar view model
        /// </summary>
        public SearchBarViewModel SearchViewModel { get; set; }
        /// <summary>
        /// All folders
        /// </summary>
        public List<SelectableFolderViewModel> AllResults { get; set; }
        /// <summary>
        /// Selectable folders matching the search
        /// </summary>
        public List<SelectableFolderViewModel> SearchResults { get; set; }
        // Currently searched text
        private string Search { get; set; }

        public FolderSelectorViewModel(NavigationPanelViewModel ParentNavigationPanel, List<SessionGroup> groups)
        {
            SearchViewModel = new SearchBarViewModel(UpdateSearchResults);
            Search = "";
            AllResults = groups.Select(
                g => new SelectableFolderViewModel(g, ParentNavigationPanel.MainView)
                ).ToList();
            UpdateSearchResults();
        }

        /// <summary>
        /// Deletes the search result about the given group
        /// </summary>
        /// <param name="group">deleted group</param>
        public void DeleteGroup(SessionGroup group)
        {
            var existingResult = AllResults.Find(res => res.Group.Equals(group));
            if (existingResult != null)
            {
                AllResults.Remove(existingResult);
                UpdateSearchResults();
            }
        }

        private void UpdateSearchResults(string search)
        {
            Search = search;
            UpdateSearchResults();
        }

        // Updates matching search result and notifies the change.
        private void UpdateSearchResults()
        {
            SearchResults = AllResults.Where(result => result.FolderName.ToLower().Trim().StartsWith(Search.ToLower().Trim()))
                                      .OrderByDescending(res => res.FolderName)
                                      .ToList();
            OnPropertyChanged(nameof(SearchResults));
        }
    }
}
