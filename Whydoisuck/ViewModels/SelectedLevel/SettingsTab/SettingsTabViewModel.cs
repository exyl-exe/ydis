using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ydis.Model.DataStructures;
using Ydis.Properties;
using Ydis.Views.Commands;

namespace Ydis.ViewModels.SelectedLevel.SettingsTab
{
    /// <summary>
    /// View model for group management
    /// </summary>
    public class SettingsTabViewModel : BaseViewModel
    {
        /// <summary>
        /// Command that deletes the group
        /// </summary>
        public DeleteGroupCommand DeleteGroup { get; private set; }

        /// <summary>
        /// Text for button deleting current group
        /// </summary>
        public string DeleteButtonText => Resources.DeleteGroupButtonText;

        // managed group
        private SessionGroup Group { get; set; }

        public SettingsTabViewModel(SessionGroup g)
        {
            Group = g;
            DeleteGroup = new DeleteGroupCommand(g);
        }
    }
}
