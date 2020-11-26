using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.SelectedLevel.SettingsTab
{
    /// <summary>
    /// View model for group management
    /// </summary>
    public class SettingsTabViewModel : BaseViewModel
    {
        /// <summary>
        /// Command that deletes the group
        /// </summary>
        public ICommand DeleteGroupCommand { get; private set; }

        // managed group
        private SessionGroup Group { get; set; }

        public SettingsTabViewModel(SessionGroup g)
        {
            Group = g;
        }
    }
}
