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

            var caption = string.Format("#Merge {0} groups", foldersCount);
            var content = string.Format("#Do you want to merge the {0} selected folders into one folder ? All of their data will be stored in the \"{1}\" folder.", foldersCount, folders[0].DisplayedName);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                Console.WriteLine("Groups to merge : " + foldersCount);
            }
        }
    }
}
