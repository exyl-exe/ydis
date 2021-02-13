using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using Ydis.Model.DataStructures;
using Ydis.ViewModels.CommonControlsViewModels;
using Ydis.ViewModels.SelectedLevel;
using Ydis.Views.Commands;
using Ydis.Views.NavigationPanel;

namespace Ydis.ViewModels.Navigation
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
        /// Search bar view model
        /// </summary>
        public SearchBarViewModel SearchViewModel { get; set; }
        /// <summary>
        /// All possible search results
        /// </summary>
        public ListCollectionView SearchResults { get; private set; }
        // Currently searched text
        private string Search { get; set; }

        public NavigationSearchViewModel(NavigationPanelViewModel ParentNavigationPanel, List<SessionGroup> groups)
        {
            SearchViewModel = new SearchBarViewModel(UpdateSearchResults);
            this.ParentNavigationPanel = ParentNavigationPanel;
            Search = "";
            var resultList = groups.Select(
                g => new NavigationSearchResultViewModel(g, ParentNavigationPanel.MainView)
                ).ToList();
            SearchResults = CollectionViewSource.GetDefaultView(resultList) as ListCollectionView;
            SearchResults.CustomSort = new ResultSorter();
            SearchResults.Filter = ResultFilter;
        }

        /// <summary>
        /// Updates the search result corresponding to a given group.
        /// </summary>
        /// <param name="group">Group to update</param>
        public void UpdateGroup(SessionGroup group)
        {
            var existingResult = FindResult(group);
            if (existingResult == null)
            {
                var newGroup = new NavigationSearchResultViewModel(group, ParentNavigationPanel.MainView);
                App.Current.Dispatcher.Invoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      SearchResults.AddNewItem(newGroup);
                      SearchResults.CommitNew();
                }));                
            } else
            {
                App.Current.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new Action(() => {
                        SearchResults.EditItem(existingResult);
                        existingResult.UpdateFromModel();
                        SearchResults.CommitEdit(); ;
                }));
                
            }
        }

        /// <summary>
        /// Deletes the search result about the given group
        /// </summary>
        /// <param name="group">deleted group</param>
        public void DeleteGroup(SessionGroup group)
        {
            var existingResult = FindResult(group);
            if (existingResult != null)
            {
                SearchResults.Remove(existingResult);
            }
        }

        // Returns wether the result matches the current search
        private bool ResultFilter(object obj)
        {
            var result = obj as NavigationSearchResultViewModel;
            return Search == "" || result.ResultText.ToLower().StartsWith(Search.ToLower());
        }

        // Updates the search criteria and notifies the change
        private void UpdateSearchResults(string search)
        {
            Search = search;
            OnPropertyChanged(nameof(Search));
            SearchResults.Refresh();
        }

        // Returns the search result of the group
        private NavigationSearchResultViewModel FindResult(SessionGroup group)
        {
            var enumerableResults = SearchResults.Cast<NavigationSearchResultViewModel>();
            var matchingResults = enumerableResults.Where(res => res.Group.Equals(group));
            return matchingResults.Any() ? matchingResults.First() : null;
        }

        private class ResultSorter : IComparer
        {
            public int Compare(object obj1, object obj2)
            {
                var res1 = obj1 as NavigationSearchResultViewModel;
                var res2 = obj2 as NavigationSearchResultViewModel;
                return res2.LastPlayedTime.CompareTo(res1.LastPlayedTime);
            }
        }
    }
}
