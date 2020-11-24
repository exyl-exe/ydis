﻿using System;
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
        /// View model for the current part of the view to display
        /// depending on if the user's playing a level or not
        /// </summary>
        public BaseViewModel CurrentView { get; private set; }
        // Recorder managing the current session
        private Recorder Recorder { get; set; }
        // View model for the place holder
        private CurrentLevelPlaceholderViewModel Placeholder { get; set; }
        // View model for statistics about the current session
        private CurrentLevelStatisticsViewModel Statistics { get; set; }

        public CurrentLevelViewModel(Recorder recorder)
        {
            Recorder = recorder;
            Placeholder = new CurrentLevelPlaceholderViewModel();
            Statistics = new CurrentLevelStatisticsViewModel();
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
            CurrentView = Statistics;
            Statistics.SetSession(s);
            var autoguess = Recorder.Autoguess == null ?
                            Resources.CurrentLevelGroupNew
                            : Recorder.Autoguess.DisplayedName;
            Statistics.SetAutoguess(autoguess);
            Title = s.Level.Name;                       
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
            Statistics.RefreshStats();
            Update();
        }

        // Resets the properties, to clean up between two sessions.
        private void SetDefaulProperties()
        {
            CurrentView = Placeholder;
            Title = Resources.CurrentLevelTitleDefault;
        }

        // Notifies the view that the viewmodel changed.
        private void Update()
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
