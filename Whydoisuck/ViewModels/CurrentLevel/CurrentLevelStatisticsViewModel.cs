using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.DataStructures;
using Ydis.Properties;
using Ydis.ViewModels.CommonControlsViewModels;
using Ydis.ViewModels.DataStructures;

namespace Ydis.ViewModels.CurrentLevel
{
    /// <summary>
    /// View model for the view showing statistics about the current session
    /// </summary>
    public class CurrentLevelStatisticsViewModel : BaseViewModel
    {
        /// <summary>
        /// Name of the group this session will belong to.
        /// </summary>
        public string Autoguess { get; set; }
        /// <summary>
        /// Statistics about the current session, as a graph
        /// </summary>
        public LevelGraphViewModel Graph { get; set; }
        /// <summary>
        /// Statistics about the current session, as a grid
        /// </summary>
        public LevelDataGridViewModel Datagrid { get; set; }
        // current session object
        private ISession Session { get; set; }
        // statistics about the current session
        private SessionsStatistics CurrentLevelStats { get; set; }

        public CurrentLevelStatisticsViewModel()
        {
            Autoguess = Resources.CurrentLevelGroupDefault;
        }

        public void SetSession(ISession s)
        {
            Session = s;
            RefreshStats();
        }

        public void SetAutoguess(string autoguess)
        {
            Autoguess = autoguess;
            OnPropertyChanged(nameof(Autoguess));
        }

        public void RefreshStats()
        {
            var currentSessionData = new SessionGroupData(Session);
            CurrentLevelStats = new SessionsStatistics(currentSessionData, 1f);
            Graph = new LevelGraphViewModel(CurrentLevelStats, Resources.GraphTitleCurrentSession);
            Datagrid = new LevelDataGridViewModel(CurrentLevelStats);
            UpdateView();
        }

        private void UpdateView()
        {
            OnPropertyChanged(nameof(Graph));
            OnPropertyChanged(nameof(Datagrid));
        }
    }
}
