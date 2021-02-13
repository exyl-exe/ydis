using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ydis.Model.DataStructures;

namespace Ydis.Views.Commands
{
    public abstract class SelectedFoldersCommand : ICommand
    {
        // Function to call to retrieve folders to delete
        protected Func<List<SessionGroup>> FoldersGetter { get; set; }

        public event EventHandler CanExecuteChanged;

        protected SelectedFoldersCommand(Func<List<SessionGroup>> getter)
        {
            FoldersGetter = getter;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);
    }
}
