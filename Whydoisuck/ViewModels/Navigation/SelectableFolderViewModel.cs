using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.Navigation
{
    public class SelectableFolderViewModel : BaseViewModel
    {
        /// <summary>
        /// Folder that can be selected
        /// </summary>
        public SessionGroup Group { get; set; }
        /// <summary>
        /// Name of the selectable folder
        /// </summary>
        public string FolderName => Group.DisplayedName;

        public SelectableFolderViewModel(SessionGroup group, MainWindowViewModel mainView)
        {     
            Group = group;
        }
    }
}
