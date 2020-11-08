using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.ViewModels.CommonControlsViewModels;
using Whydoisuck.ViewModels.DataStructures;

namespace Whydoisuck.ViewModels.CurrentLevel
{
    public class CurrentLevelViewModel : BaseViewModel
    {
        private Recorder Recorder { get; set; }
        private Session Session { get; set; }
        private SessionsStatistics CurrentLevelStats { get; set; }
        public string Title { get; set; }
        public string Autoguess { get; set; }
        public LevelGraphViewModel Graph { get; set; }
        public LevelDataGridViewModel Datagrid { get; set; }

        public CurrentLevelViewModel(Recorder recorder)
        {
            Recorder = recorder;
            SetDefaulProperties();

            Recorder.OnAttemptAdded += OnAttemptAddedToCurrent;
            Recorder.OnNewCurrentSessionInitialized += OnNewCurrentSession;
            Recorder.OnQuitCurrentSession += OnQuitCurrentSession;
        }

        public void OnNewCurrentSession(Session s)
        {
            Session = s;
            Title = s.Level.Name;
            Autoguess = Recorder.Autoguess==null?
                Properties.Resources.CurrentLevelGroupNew
                :SessionGroup.GetDefaultGroupName(s.Level);
            CurrentLevelStats = new SessionsStatistics(new List<Session>() { Session }, 1f);
            Graph = new LevelGraphViewModel(CurrentLevelStats);//TODO generating everything is bad
            Datagrid = new LevelDataGridViewModel(CurrentLevelStats);
            Update();
        }

        public void OnQuitCurrentSession(Session s)
        {
            SetDefaulProperties();
            Update();
        }

        public void OnAttemptAddedToCurrent(Attempt a)
        {
            CurrentLevelStats = new SessionsStatistics(new List<Session>() { Session }, 1f);
            Graph = new LevelGraphViewModel(CurrentLevelStats);
            Datagrid = new LevelDataGridViewModel(CurrentLevelStats);
            Update();
        }

        private void SetDefaulProperties()
        {
            Session = null;
            CurrentLevelStats = null;
            Graph = null;
            Datagrid = null;
            Title = Properties.Resources.CurrentLevelTitleDefault;
            Autoguess = Properties.Resources.CurrentLevelGroupDefault;
        }

        private void Update()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Autoguess));
            OnPropertyChanged(nameof(Graph));
            OnPropertyChanged(nameof(Datagrid));
        }
    }
}
