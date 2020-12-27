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
using Whydoisuck.DataSaving;
using Whydoisuck.Model.UserSettings;

namespace Whydoisuck.Views.Commands
{
    /// <summary>
    /// Command to choose a folder path
    /// </summary>
    public class FolderBrowserCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private SettingsViewModel AppSettings { get; set; }

        public FolderBrowserCommand(SettingsViewModel settings)
        {
            AppSettings = settings;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var path = ShowFolderPicker();
            if (path == null || path == SessionManager.Instance.SavesDirectory) return;
            WDISSettings.SavesPath = path;
            var migrateData = ShowMigrateDialog(path);
            if (migrateData)
            {
                SessionManager.Instance.SetRootAndMerge(path);
            } else
            {
                SessionManager.Instance.SetRoot(path);
            }
        }

        public string ShowFolderPicker()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.EnsurePathExists = true;
            var res = dialog.ShowDialog();
            if (res == CommonFileDialogResult.Ok)
            {
                var path = dialog.FileName;
                return path;
            } else
            {
                return null;
            }
        }

        public bool ShowMigrateDialog(string newPath)
        {
            var caption = Resources.MigrateDataCaption;
            var content = string.Format(Resources.MigrateDataContentFormat, newPath);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            return result == MessageBoxResult.Yes;
        }
    }
}
