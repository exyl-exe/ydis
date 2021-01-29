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

            var caption = string.Format(Resources.ManagementReorganizeSelectedCaptionFormat, foldersCount);
            var content = string.Format(Resources.ManagementReorganizeSelectedContentFormat, foldersCount);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                SessionManager.Instance.ReorganizeGroups(folders);
            }
        }
    }
}
