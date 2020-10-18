using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ViewModels.CommonViewModels;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    class SelectedLevelViewModel : BaseViewModel
    {
        public SessionGroup Group { get; set; }
        public GraphTabViewModel GraphTab { get; set; }
        public SessionsTabViewModel Sessions { get; set; }
        public SelectedLevelViewModel(SessionGroup g)
        {
            Group = g;
            GraphTab = new GraphTabViewModel(g);
            Sessions = new SessionsTabViewModel(g);
        }
    }
}
