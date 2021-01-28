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
            var folderCount = folders.Count();
            if (folderCount == 0) return;

            var caption = string.Format("#Merge {0} groups");
            var content = string.Format("#Do you want to merge the {0} selected groups into one group ? The resulting folder will be named \"{1}\".");
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                Console.WriteLine("Groups to merge : " + folderCount);
            }
        }
    }
}
