using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    class SelectedLevelViewModel : BaseViewModel
    {
        public SessionGroup Group { get; set; }
        public SelectedLevelViewModel(SessionGroup g)
        {
            Group = g;
        }
    }
}
