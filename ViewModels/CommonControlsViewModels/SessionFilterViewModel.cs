using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;

namespace Whydoisuck.ViewModels.CommonControlsViewModels
{
    public class SessionFilterViewModel : BaseViewModel
    {
        private bool _showNormal;
        public bool ShowNormal
        {
            get
            {
                return _showNormal;
            }
            set
            {
                _showNormal = value;
                OnFilterChanges?.Invoke();
            }
        }

        private bool _showCopy;
        public bool ShowCopy
        {
            get
            {
                return _showCopy;
            }
            set
            {
                _showCopy = value;
                OnFilterChanges?.Invoke();
            }
        }

        public delegate void OnFilterChangesCallback();
        public event OnFilterChangesCallback OnFilterChanges;

        public SessionFilterViewModel()
        {
            ShowNormal = true;
            ShowCopy = true; ;
        }

        public bool Matches(Session s)
        {
            return (s.IsCopyRun && ShowCopy) || (!s.IsCopyRun && ShowNormal);
        }
    }
}
