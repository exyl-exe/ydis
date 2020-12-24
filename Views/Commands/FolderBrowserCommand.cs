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
            if (path == null) return;            
            var migrateData = ShowMigrateDialog();
            if (migrateData)
            {
                AppSettings.MigrateData(path);
            } else
            {
                AppSettings.SetSavePath(path);
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

        public bool ShowMigrateDialog()
        {
            var caption = Resources.MigrateDataCaption;
            var content = Resources.MigrateDataContent;
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            return result == MessageBoxResult.Yes;
        }
    }
}
