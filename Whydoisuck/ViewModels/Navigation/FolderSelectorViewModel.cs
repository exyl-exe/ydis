using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
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

        private List<SelectableFolderViewModel> _folders;
        /// <summary>
        /// All folders
        /// </summary>
        public List<SelectableFolderViewModel> Folders {
            get { return _folders.OrderBy(f => f.FolderName).ToList(); }
            private set { _folders = value; }
        }

        /// <summary>
        /// Folders elected by the user
        /// </summary>
        /// <returns></returns>
        public List<SessionGroup> SelectedFolders => Folders.Where(f => f.IsSelected).Select(f => f.Group).ToList();

        // Currently searched text
        private string Search { get; set; }

        public FolderSelectorViewModel(NavigationPanelViewModel ParentNavigationPanel, List<SessionGroup> groups)
        {
            SearchViewModel = new SearchBarViewModel(UpdateSearchResults);
            Search = "";
            Folders = groups.Select(
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
            var existingResult = Folders.Find(res => res.Group.Equals(group));
            if (existingResult != null)
            {
                Folders.Remove(existingResult);
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
            foreach(var f in Folders)
            {
                var isVisible = f.FolderName.ToLower().Trim().StartsWith(Search.ToLower().Trim())
                                || f.IsSelected;
                f.IsVisible = isVisible;
            }
        }
    }
}
