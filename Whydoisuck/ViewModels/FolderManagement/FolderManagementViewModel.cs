using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.FolderManagement
{
    /// <summary>
    /// View model for the folder management view
    /// </summary>
    public class FolderManagementViewModel : BaseViewModel
    {
        /// <summary>
        /// Command to quit the folder management view
        /// </summary>
        public NavigatorCommand GoBackCommand { get; set; }

        public FolderManagementViewModel(MainWindowViewModel mainWindow, CurrentLevelViewModel currentLevel)
        {
            GoBackCommand = new NavigatorCommand(mainWindow, currentLevel);
        }
    }
}
