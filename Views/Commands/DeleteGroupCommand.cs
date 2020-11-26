using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.Views.Commands
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
            SessionManager.Instance.DeleteGroup(Group);
        }
    }
}
