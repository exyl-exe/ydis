using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ydis.Model.DataSaving;
using Ydis.Model.DataStructures;
using Ydis.Properties;

namespace Ydis.Views.Commands
{
    public class MergeSelectedCommand : SelectedFoldersCommand
    {
        public MergeSelectedCommand(Func<List<SessionGroup>> getter) : base(getter) { }

        public override void Execute(object parameter)
        {
            var folders = FoldersGetter();
            var foldersCount = folders.Count();
            if (foldersCount < 2) return;

            var root = SessionManager.Instance.GetMergingRoot(folders);
            var caption = string.Format(Resources.ManagementMergeFoldersCaptionFormat, foldersCount);
            var content = string.Format(Resources.ManagementMergeFoldersContentFormat, foldersCount, root.DisplayedName);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                SessionManager.Instance.MergeGroups(folders);
            }
        }
    }
}
