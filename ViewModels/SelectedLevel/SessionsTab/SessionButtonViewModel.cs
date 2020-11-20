using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Views.Commands;

namespace Whydoisuck.ViewModels.SelectedLevel.SessionsTab
{
    /// <summary>
    /// View model for buttons that open a detailled summary of a session
    /// </summary>
    public class SessionButtonViewModel : BaseViewModel
    {
        private bool _visible;
        /// <summary>
        /// Wether the button should be displayed or not
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                OnPropertyChanged(nameof(Visible));
                OnPropertyChanged(nameof(Visibility));
            }
        }

        /// <summary>
        /// Session that this button will open a summary of
        /// </summary>
        public Session Session { get; }
        /// <summary>
        /// Visibility for WPF controls.
        /// </summary>
        public Visibility Visibility => Visible ? Visibility.Visible : Visibility.Collapsed;
        /// <summary>
        /// Command that opens the summary of a session
        /// </summary>
        public ICommand ViewSessionCommand { get; set; }

        public SessionButtonViewModel(SessionsTabMainViewModel parent, Session session)
        {
            Session = session;
            Visible = true;
            ViewSessionCommand = new ViewSessionCommand(parent, session);
        }
    }
}
