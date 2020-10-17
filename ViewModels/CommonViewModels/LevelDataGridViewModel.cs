using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel;
using Whydoisuck.ModelAgain;
using Whydoisuck.UIModel;

namespace Whydoisuck.ViewModels.CommonViewModels
{
    public class LevelDataGridViewModel : BaseViewModel
    {
        public SessionsStatistics SessionStats { get; set; }
        public LevelDataGridViewModel(SessionGroup group)
        {
            SessionStats = new SessionsStatistics(group.GroupSessions, 1f);
        }
    }
}
