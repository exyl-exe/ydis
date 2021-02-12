using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.Model.DataSaving;

namespace Whydoisuck.Views.Commands
{
    public class MoveDataCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<string, bool> Callback { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var path = PathDialog();
            if (path == null || path == SessionManager.Instance.SavesDirectory) return;
            SessionManager.Instance.Import(path);
        }

        private string PathDialog()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.EnsurePathExists = true;
            var res = dialog.ShowDialog();
            if (res == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
