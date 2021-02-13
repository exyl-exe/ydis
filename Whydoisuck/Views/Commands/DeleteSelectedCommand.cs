using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ydis.Model.DataSaving;
using Ydis.Model.DataStructures;
using Ydis.Properties;

namespace Ydis.Views.Commands
{
    /// <summary>
    /// Command to delete a bunch of folders
    /// </summary>
    public class DeleteSelectedCommand : SelectedFoldersCommand
    {
        public DeleteSelectedCommand(Func<List<SessionGroup>> getter) : base(getter){ }

        public override void Execute(object parameter)
        {
            var folders = FoldersGetter();
            var folderCount = folders.Count();
            if (folderCount == 0) return;

            var caption = string.Format(Resources.ManagementDeleteFoldersCaptionFormat, folderCount);
            var content = string.Format(Resources.ManagementDeleteFoldersContentFormat, folderCount);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                foreach(var group in folders)
                {
                    SessionManager.Instance.DeleteGroup(group);
                }
            }
        }
    }
}
