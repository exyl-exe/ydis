using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Whydoisuck.DataModel;

namespace Whydoisuck.ViewModels.SelectedLevel
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
        public SessionButtonViewModel(Session session)
        {
            Session = session;
            Visible = true;
        }
    }
}
