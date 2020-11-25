using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.DataViewModels;
using Whydoisuck.ViewModels.SelectedLevel.GraphTab;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab;

namespace Whydoisuck.ViewModels.SelectedLevel
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
        // Selected folder
        private SessionGroup SelectedLevel { get; set; }

        public SelectedLevelViewModel(SessionGroup g)
        {
            Group = new GroupViewModel(g);
            GraphTab = new GraphTabMainViewModel(g);
            Sessions = new SessionsTabMainViewModel(g);
            SelectedLevel = g;
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
