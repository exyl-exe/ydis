using System.Collections.Generic;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Properties;
using Whydoisuck.ViewModels.CurrentLevel;
using Whydoisuck.ViewModels.Navigation;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.FolderManagement
{
    public class FolderManagementViewModel : BaseViewModel
    {
        /// <summary>
        /// Folder management view title
        /// </summary>
        public string Title => Resources.FolderManagementTitle;
        /// <summary>
        /// Label on the merge button
        /// </summary>
        public string MergeButtonText => Resources.FolderManagementMergeButton;
        /// <summary>
        /// Label on the delete button
        /// </summary>
        public string DeleteButtonText => Resources.FolderManagementDeleteButton;
        /// <summary>
        /// Label on the reorganize button
        /// </summary>
        public string ReorganizeButtonText => Resources.FolderManagementReorganizeButton;
        /// <summary>
        /// Label on the reorganize all button
        /// </summary>
        public string ReorganizeAllButtonText => Resources.FolderManagementReorganizeAllButton;

        /// <summary>
        /// Command to quit folder management view
        /// </summary>
        public NavigatorCommand GoBackCommand { get; set; }
        /// <summary>
        /// Command to delete all selected folders
        /// </summary>
        public DeleteSelectedCommand DeleteSelectedCommand { get; private set; }
        /// <summary>
        /// Command to merge all selected folders
        /// </summary>
        public MergeSelectedCommand MergeSelectedCommand { get; private set; }

        private MainWindowViewModel MainWindow { get; set; }
        private CurrentLevelViewModel CurrentSession { get; set; }
        private FolderSelectorViewModel FolderSelector { get; set; }

        public FolderManagementViewModel(MainWindowViewModel mainWindow, CurrentLevelViewModel currentSession, FolderSelectorViewModel folderSelector)
        {
            MainWindow = mainWindow;
            CurrentSession = currentSession;
            FolderSelector = folderSelector;

            GoBackCommand = new NavigatorCommand(MainWindow, CurrentSession);
            DeleteSelectedCommand = new DeleteSelectedCommand(FolderSelector.GetSelectedFolders);
            MergeSelectedCommand = new MergeSelectedCommand(FolderSelector.GetSelectedFolders);
        }
    }
}