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
        public string Autoguess { get; set; }//TODO
        public LevelGraphViewModel Graph { get; set; }
        public LevelDataGridViewModel Datagrid { get; set; }

        public CurrentLevelViewModel(Recorder recorder)
        {
            Recorder = recorder;
            Session = null;
            CurrentLevelStats = null;
            Title = "Not playing any level TODO";//TODO
            Autoguess = "None TODO";//TODO

            Recorder.OnAttemptAdded += OnAttemptAddedToCurrent;
            Recorder.OnNewCurrentSessionInitialized += OnNewCurrentSession;
            Recorder.OnQuitCurrentSession += OnQuitCurrentSession;
        }

        public void OnNewCurrentSession(Session s)
        {
            Session = s;
            Title = s.Level.Name;
            Autoguess = Recorder.Manager.FindGroupOf(s.Level)==null?"New group TODO":SessionGroup.GetDefaultGroupName(s.Level);//TODO
            CurrentLevelStats = new SessionsStatistics(new List<Session>() { Session }, 1f);
            Graph = new LevelGraphViewModel(CurrentLevelStats.Statistics);//TODO
            Datagrid = new LevelDataGridViewModel(CurrentLevelStats.Statistics);
            Update();
        }

        public void OnQuitCurrentSession(Session s)
        {
            Session = null;
            CurrentLevelStats = null;
            Graph = null;
            Datagrid = null;
            Title = "Not playing any level TODO";//TODO
            Autoguess = "None TODO";//TODO
            Update();
        }

        public void OnAttemptAddedToCurrent(Attempt a)
        {
            CurrentLevelStats = new SessionsStatistics(new List<Session>() { Session }, 1f);
            Graph = new LevelGraphViewModel(CurrentLevelStats.Statistics);//TODO
            Datagrid = new LevelDataGridViewModel(CurrentLevelStats.Statistics);
            Update();
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
