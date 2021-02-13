using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ydis.Model.DataStructures;

namespace Ydis.ViewModels.Navigation
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
        private bool _selected;
        /// <summary>
        /// Wether the widget is selected or not
        /// </summary>
        public bool IsSelected {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value; OnPropertyChanged(nameof(IsSelected));
            }
        }

        public SelectableFolderViewModel(SessionGroup group)
        {     
            Group = group;
        }
    }
}
