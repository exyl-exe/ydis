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
    public class SessionButtonViewModel : BaseViewModel
    {
        private bool _visible;
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

        public Session Session { get; }
        public Visibility Visibility => Visible ? Visibility.Visible : Visibility.Collapsed;
        public ICommand ViewSessionCommand { get; set; }

        public SessionButtonViewModel(SessionsTabMainViewModel parent, Session session)
        {
            Session = session;
            Visible = true;
            ViewSessionCommand = new ViewSessionCommand(parent, session);
        }
    }
}
