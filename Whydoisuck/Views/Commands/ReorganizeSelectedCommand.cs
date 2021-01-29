using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Properties;

namespace Whydoisuck.Views.Commands
{
    public class ReorganizeSelectedCommand : SelectedFoldersCommand
    {
        public ReorganizeSelectedCommand(Func<List<SessionGroup>> getter) : base(getter) { }

        public override void Execute(object parameter)
        {
            var folders = FoldersGetter();
            var foldersCount = folders.Count();
            if (foldersCount < 1) return;

            var caption = "#Reorganize selected folders ";//string.Format(Resources.ManagementMergeFoldersCaptionFormat, foldersCount);
            var content = "#Do you want to reorganize the selected folders ? Their data will be reorganized using the default criteria. This is used to undo merging operations.";//string.Format(Resources.ManagementMergeFoldersContentFormat, foldersCount);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                SessionManager.Instance.ReorganizeGroups(folders);
            }
        }
    }
}
