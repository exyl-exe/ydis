using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.UserSettings;
using Whydoisuck.Properties;

namespace Whydoisuck.Views.Commands
{
    public class ChangeSavesLocationCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var path = PathDialog();
            if (path == null || path == SessionManager.Instance.SavesDirectory) return;
            var currentPath = WDISSettings.SavesPath;
            var shouldMoveData = ShouldMoveData();
            WDISSettings.SavesPath = path;
            SessionManager.Instance.SetRoot(path);
            if (shouldMoveData)
            {
                SessionManager.Instance.Import(currentPath);
                DataSerializer.DeleteSaveFiles(currentPath);
            }

        }

        private string PathDialog()
        {
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                EnsurePathExists = true
            };
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

        private bool ShouldMoveData()
        {
            var caption = Resources.SettingsMoveDataCaption;
            var content = Resources.SettingsMoveDataContent;
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            return result == MessageBoxResult.Yes;
        }
    }
}
