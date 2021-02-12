using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using Whydoisuck.Properties;
using Whydoisuck.ViewModels.AppSettings;
using Whydoisuck.Model.UserSettings;
using System.IO;
using System.Runtime.CompilerServices;

namespace Whydoisuck.Views.Commands
{
    /// <summary>
    /// Command to choose a folder path
    /// </summary>
    public class FolderBrowserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<string> Callback { get; set; }

        public FolderBrowserCommand(Action<string> OnSelectCallback)
        {
            Callback = OnSelectCallback;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                EnsurePathExists = true
            };
            var res = dialog.ShowDialog();
            if (res == CommonFileDialogResult.Ok)
            {
                Callback(dialog.FileName);
            } else
            {
                Callback(null);
            }
        }
    }
}
