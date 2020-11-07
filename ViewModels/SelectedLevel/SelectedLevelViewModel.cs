using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.SelectedLevel.GraphTab;
using Whydoisuck.ViewModels.SelectedLevel.SessionsTab;

namespace Whydoisuck.ViewModels.SelectedLevel
{
    public class SelectedLevelViewModel : BaseViewModel
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

        public void Update()
        {
            GraphTab = new GraphTabMainViewModel(Group);
            Sessions = new SessionsTabMainViewModel(Group);
            OnPropertyChanged(nameof(GraphTab));
            OnPropertyChanged(nameof(Sessions));
        }
    }
}
