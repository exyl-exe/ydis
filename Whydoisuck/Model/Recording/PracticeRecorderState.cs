using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.MemoryReading.GameStateStructures;

namespace Whydoisuck.Model.Recording
{
    /// <summary>
    /// Recorder state for practice mode
    /// </summary>
    public class PracticeRecorderState : IRecorderState
    {
        public SessionGroup Autoguess
        {
            get
            {
                if (CurrentSession != null)
                {
                    return null;
                }
                return null;
            }
        }

        public event Action<ISession> OnSessionInitialized;
        public event Action<ISession> OnQuitSession;
        public event Action OnAttemptsUpdated;

        private PracticeSession CurrentSession { get; set; }
        private bool _isCurrentAttemptSaved;
        private PracticeAttempt CurrentAttempt { get; set; }

        public void OnSessionStarted(GameState state)
        {
            if (state == null || state.LoadedLevel == null || !state.LoadedLevel.IsRunning) return;

            CurrentSession = new PracticeSession(DateTime.Now);
            CurrentAttempt = null;
            _isCurrentAttemptSaved = false;
            CurrentSession.Level = new Level(state);
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;

            if (!state.PlayerObject.IsDead)
            {
                var number = state.LoadedLevel.AttemptNumber;
                var firstStartPercent = state.PlayerObject.XPosition * 100 / state.LoadedLevel.PhysicalLength;
                CurrentAttempt = new PracticeAttempt(number, firstStartPercent);
            }

            OnSessionInitialized?.Invoke(CurrentSession);
        }

        public void OnSessionEnded(GameState state)
        {
            if (CurrentSession == null || CurrentSession.Attempts.Count == 0) return;
            CurrentSession.Duration = DateTime.Now - CurrentSession.StartTime;
            SessionManager.Instance.SavePracticeSession(CurrentSession);
            OnQuitSession?.Invoke(CurrentSession);
            CurrentSession = null;
            CurrentAttempt = null;
            _isCurrentAttemptSaved = true;
        }

        public void OnAttemptStarted(GameState state)
        {
            var number = state.LoadedLevel.AttemptNumber;
            var start = 100 * state.LoadedLevel.PracticeStartPosition / state.LoadedLevel.PhysicalLength;
            CurrentAttempt = new PracticeAttempt(number, start);
            _isCurrentAttemptSaved = false;
        }

        public void OnAttemptEnded(GameState state)
        {
            if (state == null || state.PlayerObject == null) return;
            float endPercent = 0;
            if (state.PlayerObject.HasWon)
            {
                endPercent = 100;
            }
            else
            {
                endPercent = 100 * state.PlayerObject.XPosition / state.LoadedLevel.PhysicalLength;
            }
            SaveCurrentAttempt(state, endPercent);
        }

        // Saves the current attempt with the specified end percent
        private void SaveCurrentAttempt(GameState state, float endPercent)
        {
            if (!_isCurrentAttemptSaved)
            {
                CreateSessionIfNotExists(state);
                CreateAttemptIfNotExists(state);
                CurrentAttempt.EndPercent = endPercent;
                CurrentSession.AddAttempt(CurrentAttempt);
                CurrentSession.Duration = DateTime.Now - CurrentSession.StartTime; //Updating duration for UI
                OnAttemptsUpdated?.Invoke();
            }
            CurrentAttempt = null;
            _isCurrentAttemptSaved = true;
        }

        //Creates a session if there is no current session and initialize known values
        private void CreateSessionIfNotExists(GameState state)
        {
            if (CurrentSession != null) return;
            CurrentSession = new PracticeSession(DateTime.Now);

            if (state == null || state.LevelMetadata == null || state.LoadedLevel == null) return;
            CurrentSession.Level = new Level(state); // TODO Duplicate code
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;
            OnSessionInitialized?.Invoke(CurrentSession);
        }

        //Creates an attempt if there is no current attempt and initialize known values
        private void CreateAttemptIfNotExists(GameState state)
        {
            if (CurrentAttempt != null) return;
            var number = state.LoadedLevel.AttemptNumber;
            var start = 100 * state.LoadedLevel.PracticeStartPosition / state.LoadedLevel.PhysicalLength;
            CurrentAttempt = new PracticeAttempt(number, start);
            _isCurrentAttemptSaved = false;

            if (state.LoadedLevel == null) return;
            CurrentAttempt.Number = state.LoadedLevel.AttemptNumber;
        }
    }
}
