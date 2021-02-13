using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.DataStructures;
using Ydis.ViewModels.CommonControlsViewModels;
using Ydis.ViewModels.DataViewModels;
using Ydis.ViewModels.SelectedLevel.GraphTab;
using Ydis.ViewModels.SelectedLevel.SessionsTab;
using Ydis.ViewModels.SelectedLevel.SettingsTab;

namespace Ydis.ViewModels.SelectedLevel
{
    /// <summary>
    /// ViewModel for selected level view.
    /// </summary>
    public class SelectedLevelViewModel : BaseViewModel
    {
        /// <summary>
        /// ViewModel for the currently selected level.
        /// </summary>
        public GroupViewModel Group { get; set; }
        /// <summary>
        /// ViewModel for the statistics tab
        /// </summary>
        public GraphTabMainViewModel GraphTab { get; set; }
        /// <summary>
        /// ViewModel for the sessions tab
        /// </summary>
        public SessionsTabMainViewModel Sessions { get; set; }
        /// <summary>
        /// View model for the settings of the group
        /// </summary>
        public SettingsTabViewModel Settings { get; set; }
        // Selected folder
        private SessionGroup SelectedLevel { get; set; }

        public SelectedLevelViewModel(SessionGroup g)
        {
            Group = new GroupViewModel(g);
            GraphTab = new GraphTabMainViewModel(g);
            Sessions = new SessionsTabMainViewModel(g);
            Settings = new SettingsTabViewModel(g);
            SelectedLevel = g;
            SelectedLevel.OnSessionsChange += Update;
        }

        /// <summary>
        /// Returns wether the given group is the one currently managed by the view model.
        /// </summary>
        public bool ContainsGroup(SessionGroup g)
        {
            return SelectedLevel.Equals(g);
        }

        /// <summary>
        /// Updates the view so that it matches its associated group.
        /// Called when the group was modified.
        /// </summary>
        public void Update()
        {
            GraphTab = new GraphTabMainViewModel(SelectedLevel);
            Sessions = new SessionsTabMainViewModel(SelectedLevel);
            OnPropertyChanged(nameof(GraphTab));
            OnPropertyChanged(nameof(Sessions));
        }
    }
}
