using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.SelectedLevel.GraphTab;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    class SelectedLevelViewModel : BaseViewModel
    {
        public SessionGroup Group { get; set; }
        public GraphTabMainViewModel GraphTab { get; set; }
        public SessionsTabMainViewModel Sessions { get; set; }

        public SelectedLevelViewModel(SessionGroup g)
        {
            Group = g;
            GraphTab = new GraphTabMainViewModel(g);
            Sessions = new SessionsTabMainViewModel(g);
        }
    }
}
