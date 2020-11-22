using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Properties;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.CurrentLevel
{
    /// <summary>
    /// View model for the model of the view for the current session
    /// </summary>
    public class CurrentLevelViewModel : BaseViewModel
    {
        /// <summary>
        /// Name of the current session.
        /// </summary>
        public string Title { get; set; }
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
        // Recorder managing the current session
        private Recorder Recorder { get; set; }
        // current session object
        private Session Session { get; set; }
        // statistics about the current session
        private SessionsStatistics CurrentLevelStats { get; set; }

        public CurrentLevelViewModel(Recorder recorder)
        {
            Recorder = recorder;
            SetDefaulProperties();

            Recorder.OnAttemptAdded += OnAttemptAddedToCurrent;
            Recorder.OnNewCurrentSessionInitialized += OnNewCurrentSession;
            Recorder.OnQuitCurrentSession += OnQuitCurrentSession;
        }

        /// <summary>
        /// Callback to update the view if the recorder creates a new current session
        /// </summary>
        /// <param name="s">The session created by the recorder</param>
        public void OnNewCurrentSession(Session s)
        {
            Session = s;
            Title = s.Level.Name;
            Autoguess = Recorder.Autoguess==null?
                        Resources.CurrentLevelGroupNew
                        :SessionGroup.GetDefaultGroupName(s.Level);
            CurrentLevelStats = new SessionsStatistics(new List<Session>() { Session }, 1f);
            Graph = new LevelGraphViewModel(CurrentLevelStats, Resources.GraphTitleCurrentSession);
            Datagrid = new LevelDataGridViewModel(CurrentLevelStats);
            Update();
        }

        /// <summary>
        /// Callback to update the view if the current session has ended
        /// </summary>
        /// <param name="s">The session that just ended.</param>
        public void OnQuitCurrentSession(Session s)
        {
            SetDefaulProperties();
            Update();
        }

        /// <summary>
        /// Callback to update the view when a new attempt is added to the current session
        /// </summary>
        /// <param name="a"></param>
        public void OnAttemptAddedToCurrent(Attempt a)
        {
            CurrentLevelStats = new SessionsStatistics(new List<Session>() { Session }, 1f);
            Graph = new LevelGraphViewModel(CurrentLevelStats, Resources.GraphTitleCurrentSession);
            Datagrid = new LevelDataGridViewModel(CurrentLevelStats);
            Update();
        }

        // Resets the properties, to clean up between two sessions.
        private void SetDefaulProperties()
        {
            Session = null;
            CurrentLevelStats = null;
            Graph = null;
            Datagrid = null;
            Title = Resources.CurrentLevelTitleDefault;
            Autoguess = Resources.CurrentLevelGroupDefault;
        }

        // Notifies the view that the viewmodel changed.
        private void Update()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Autoguess));
            OnPropertyChanged(nameof(Graph));
            OnPropertyChanged(nameof(Datagrid));
        }
    }
}
