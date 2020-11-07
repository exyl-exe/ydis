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
        public bool ShowNormal { get; set; }
        public bool ShowCopy { get; set; }

        public delegate void OnFilterChangesCallback();
        public event OnFilterChangesCallback OnFilterChanges;

        public SessionFilterViewModel()
        {
            ShowNormal = true;
            ShowCopy = false;
        }

        public bool Matches(Session s)
        {
            return (s.IsCopyRun && ShowCopy) || (!s.IsCopyRun && ShowNormal);
        }
    }
}
