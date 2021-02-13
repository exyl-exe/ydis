using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Ydis.Model.DataSaving;
using Ydis.Model.DataStructures;
using Ydis.Properties;

namespace Ydis.Views.Commands
{
    /// <summary>
    /// Command to delete a group when activated
    /// </summary>
    public class DeleteGroupCommand : ICommand
    {
        private SessionGroup Group { get; set; }
        public DeleteGroupCommand(SessionGroup g)
        {
            Group = g;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var caption = string.Format(Resources.DeleteGroupCaptionFormat, Group.DisplayedName);
            var content = string.Format(Resources.DeleteGroupContentFormat, Group.DisplayedName);
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if(result == MessageBoxResult.Yes)
            {
                SessionManager.Instance.DeleteGroup(Group);
            }
        }
    }
}
