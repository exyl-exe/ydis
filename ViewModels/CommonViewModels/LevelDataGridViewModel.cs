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
    public class LevelDataGridViewModel
    {
        private readonly SessionsStatistics SessionStats;
        public List<LevelPercentData> PercentsData {
            get { return SessionStats.Statistics; }
        }
        public LevelDataGridViewModel(SessionGroup group)
        {
            SessionStats = new SessionsStatistics(group.GroupSessions, 1f);
        }
    }
}
