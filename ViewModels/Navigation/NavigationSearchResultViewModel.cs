using System;
using System.Collections.Generic;
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
    public class NavigationSearchResultViewModel
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
        /// How the group name shoudl be displayed in the search result
        /// </summary>
        public string ResultText => Group.GroupName;

        // ViewModel the main view should switch to
        private SelectedLevelViewModel SelectedView { get; set; }

        public NavigationSearchResultViewModel(SessionGroup group, MainWindowViewModel mainView, SelectedLevelViewModel selectedView)
        {
            Group = group;
            SelectedView = selectedView;
            UpdateCommand = new NavigatorCommand(mainView, selectedView);
        }

        /// <summary>
        /// Updates the corresponding selected view view model to match the session group.
        /// </summary>
        public void Update()
        {
            SelectedView.Update();
        }
    }
}
