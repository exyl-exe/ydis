using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.Views.Commands
{
    public class DeleteSelectedCommand : ICommand
    {
        private Func<List<SessionGroup>> GroupGetter { get; set; }

        public DeleteSelectedCommand(Func<List<SessionGroup>> getter)
        {
            GroupGetter = getter;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var caption = "#Delete all selected groups ?";
            var content = "#Delete all selected groups ? this is irreversible";
            var result = MessageBox.Show(content, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                var groups = GroupGetter();
                foreach(var group in groups)
                {
                    SessionManager.Instance.DeleteGroup(group);
                }
            }
        }
    }
}
