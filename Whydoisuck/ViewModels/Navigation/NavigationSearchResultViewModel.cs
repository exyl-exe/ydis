using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.SelectedLevel;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.Navigation
{
    /// <summary>
    /// View model for one result of the group search in the navigation panel
    /// </summary>
    public class NavigationSearchResultViewModel : BaseViewModel, IEditableObject
    {
        /// <summary>
        /// Command that switched the main view when the search result is clicked
        /// </summary>
        public NavigatorCommand UpdateCommand { get; set; }
        /// <summary>
        /// Group the selected level view should be about once the search result is clicked
        /// </summary>
        public SessionGroup Group { get; set; }
        /// <summary>
        /// How the group name should be displayed in the search result
        /// </summary>
        public string ResultText => Group.DisplayedName;
        /// <summary>
        /// Last time on of the folder's levels was played
        /// </summary>
        public DateTime LastPlayedTime => Group.LastPlayedTime;

        // ViewModel the main view should switch to
        private DelayedViewModel SelectedView { get; set; }

        public NavigationSearchResultViewModel(SessionGroup group, MainWindowViewModel mainView)
        {
            var selectedView = new DelayedViewModel(() => new SelectedLevelViewModel(group));
            Group = group;
            Group.OnDisplayedNameChanges += UpdateName;
            SelectedView = selectedView;
            UpdateCommand = new NavigatorCommand(mainView, selectedView);
        }

        /// <summary>
        /// Updates the corresponding view model to match the session group.
        /// </summary>
        public override void UpdateFromModel()
        {
            SelectedView.UpdateFromModel();
        }

        // Updates the text to display for this seacrh result
        private void UpdateName()
        {
            OnPropertyChanged(nameof(ResultText));
        }

        public void BeginEdit()
        {
            
        }

        public void EndEdit()
        {
            
        }

        public void CancelEdit()
        {
            
        }
    }
}
