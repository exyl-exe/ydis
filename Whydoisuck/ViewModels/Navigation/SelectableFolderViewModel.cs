using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private bool _visible;
        /// <summary>
        /// Wether the widget is hidden or shown
        /// </summary>
        public bool IsVisible
        {
            get => _visible;
            set
            {
                _visible = value;
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(Visibility));
            }
        }

        /// <summary>
        /// Visibility for WPF controls.
        /// </summary>
        public Visibility Visibility => IsVisible ? Visibility.Visible : Visibility.Collapsed;

        public SelectableFolderViewModel(SessionGroup group, MainWindowViewModel mainView)
        {     
            Group = group;
        }
    }
}
