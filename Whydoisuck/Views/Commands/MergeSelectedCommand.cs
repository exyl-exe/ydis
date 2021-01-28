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
    public class MergeSelectedCommand : SelectedFoldersCommand
    {
        public MergeSelectedCommand(Func<List<SessionGroup>> getter) : base(getter) { }

        public override void Execute(object parameter)
        {
            var folders = FoldersGetter();
            var foldersCount = folders.Count();
            if (foldersCount < 2) return;

            var caption = string.Format(Resources.ManagementMergeFoldersCaptionFormat, foldersCount);
            var content = string.Format(Resources.ManagementMergeFoldersContentFormat, foldersCount, "#Placeholder");
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                
            }
        }
    }
}
