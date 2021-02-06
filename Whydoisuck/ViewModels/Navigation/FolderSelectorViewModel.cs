using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Navigation;
using System.Windows.Threading;
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
        public ListCollectionView Folders { get; private set; }

        // Currently searched text
        private string Search { get; set; }

        public FolderSelectorViewModel(List<SessionGroup> groups)
        {
            SearchViewModel = new SearchBarViewModel(UpdateSearchResults);
            Search = "";
            var folderList = groups.Select(
                g => new SelectableFolderViewModel(g)
                ).ToList();
            Folders = CollectionViewSource.GetDefaultView(folderList) as ListCollectionView;
            Folders.CustomSort = new FolderSorter();
            Folders.Filter = FolderFilter;
        }

        /// <summary>
        /// getter for the selected folders.
        /// </summary>
        public List<SessionGroup> GetSelectedFolders()
        {
            return Folders.Cast<SelectableFolderViewModel>()
                    .Where(f => f.IsSelected)
                    .Select(f => f.Group)
                    .ToList();
        }

        /// <summary>
        /// Unselects all selected folders
        /// </summary>
        public void ResetSelection()
        {
            foreach(var f in Folders.Cast<SelectableFolderViewModel>())
            {
                f.IsSelected = false;
            }
        }

        /// <summary>
        /// Updates or creates the result matching the given group
        /// </summary>
        public void UpdateGroup(SessionGroup group)
        {
            var existingResult = FindSelectableFolder(group);
            if (existingResult == null)
            {
               var newGroup = new SelectableFolderViewModel(group);
               App.Current.Dispatcher.Invoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      Folders.AddNewItem(newGroup);
                      Folders.CommitNew();
                  }));
            }
            else
            {                
                var newGroup = new SelectableFolderViewModel(group);
                App.Current.Dispatcher.Invoke(
                   DispatcherPriority.Background,
                   new Action(() => {
                       Folders.EditItem(existingResult);
                       existingResult.UpdateFromModel();
                       Folders.CommitEdit();
                   }));
            }
        }

        /// <summary>
        /// Deletes the search result about the given group
        /// </summary>
        /// <param name="group">deleted group</param>
        public void DeleteGroup(SessionGroup group)
        {
            var existingResult = FindSelectableFolder(group);
            if (existingResult != null)
            {
                Folders.Remove(existingResult);
            }
        }

        // Returns wether a folder hsould be displayed or not
        private bool FolderFilter(object obj)
        {
            var f = obj as SelectableFolderViewModel;
            return Search == ""
                || f.FolderName.ToLower().Trim().StartsWith(Search.ToLower().Trim())                               
                || f.IsSelected;
        }

        // Updates the search criteria and the displayed folders
        private void UpdateSearchResults(string search)
        {
            Search = search;
            OnPropertyChanged(nameof(Search));
            Folders.Refresh();
        }

        // Finds the selectable folder about a specified folder
        private SelectableFolderViewModel FindSelectableFolder(SessionGroup group)
        {
            var enumerable = Folders.Cast<SelectableFolderViewModel>();
            var matchingFolders = enumerable.Where(res => res.Group.Equals(group));
            return matchingFolders.Any() ? matchingFolders.First() : null;
        }

        private class FolderSorter : IComparer
        {
            public int Compare(object obj1, object obj2)
            {
                var res1 = obj1 as SelectableFolderViewModel;
                var res2 = obj2 as SelectableFolderViewModel;
                return res1.FolderName.CompareTo(res2.FolderName);
            }
        }
    }
}
