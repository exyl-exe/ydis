using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<SelectableFolderViewModel> _folders;
        /// <summary>
        /// All folders
        /// </summary>
        public ObservableCollection<SelectableFolderViewModel> Folders {
            get { return _folders; }
            private set { _folders = new ObservableCollection<SelectableFolderViewModel>(value.OrderBy(f => f.FolderName).ToList()); }
        }

        /// <summary>
        /// Folders elected by the user
        /// </summary>
        /// <returns></returns>
        public List<SessionGroup> SelectedFolders => Folders.Where(f => f.IsSelected).Select(f => f.Group).ToList();


        private NavigationPanelViewModel ParentNavigationPanel { get; set; }
        // Currently searched text
        private string Search { get; set; }

        public FolderSelectorViewModel(NavigationPanelViewModel ParentNavigationPanel, List<SessionGroup> groups)
        {
            SearchViewModel = new SearchBarViewModel(UpdateSearchResults);
            this.ParentNavigationPanel = ParentNavigationPanel;
            Search = "";
            Folders = new ObservableCollection<SelectableFolderViewModel>(groups.Select(
                g => new SelectableFolderViewModel(g, ParentNavigationPanel.MainView)
                ).ToList());
            UpdateSearchResults();
        }

        /// <summary>
        /// getter for the selected folders.
        /// </summary>
        public List<SessionGroup> GetSelectedFolders()
        {
            return SelectedFolders;
        }

        /// <summary>
        /// Unselects all selected folders
        /// </summary>
        public void ResetSelection()
        {
            foreach(var f in Folders)
            {
                f.IsSelected = false;
            }
        }

        /// <summary>
        /// Updates or creates the result matching the given group
        /// </summary>
        public void UpdateGroup(SessionGroup group)
        {
            var existingResult = Folders.ToList().Find(res => res.Group.Equals(group));
            if (existingResult == null)
            {
                var newGroup = new SelectableFolderViewModel(group, ParentNavigationPanel.MainView);
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    Folders.Add(newGroup);
                });                
            }
            else
            {
                existingResult.UpdateFromModel();
            }
            UpdateSearchResults();
        }

        /// <summary>
        /// Deletes the search result about the given group
        /// </summary>
        /// <param name="group">deleted group</param>
        public void DeleteGroup(SessionGroup group)
        {
            var existingResult = Folders.ToList().Find(res => res.Group.Equals(group));
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
            };
            OnPropertyChanged(nameof(Folders));
        }       
    }
}
