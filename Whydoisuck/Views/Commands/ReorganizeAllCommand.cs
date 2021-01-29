using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Properties;

namespace Whydoisuck.Views.Commands
{
    public class ReorganizeAllCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var caption = Resources.ManagementReorganizeAllCaption;
            var content = Resources.ManagementReorganizeAllContent;
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                SessionManager.Instance.ReorganizeAll();
            }
        }
    }
}
