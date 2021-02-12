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
    /// State for the recorder when recordings normal mod attempts
    /// </summary>
    public class NormalRecorderState : IRecorderState
    {
        public event Action<ISession> OnSessionInitialized;
        public event Action<ISession> OnQuitSession;
        public event Action OnAttemptsUpdated;

        // Wether the current attempt has been saved or not
        private bool _isCurrentAttemptSaved;
        // Attempt whose data is currently being recorded
        private Attempt CurrentAttempt { get; set; }
        /// <summary>
        /// Current session on the currently played level.
        /// </summary>
        private Session CurrentSession { get; set; }

        /// <summary>
        /// Guessed group for the current session.
        /// </summary>
        public SessionGroup Autoguess
        {
            get
            {
                if (CurrentSession != null)
                {
                    return SessionManager.Instance.FindGroupOf(CurrentSession.Level);
                }
                return null;
            }
        }

        public void OnSessionStarted(GameState state)
        {
            if (state == null || state.LoadedLevel == null || !state.LoadedLevel.IsRunning) return;

            CurrentSession = new Session(DateTime.Now);
            CurrentAttempt = null;
            _isCurrentAttemptSaved = false;
            CurrentSession.Level = new Level(state);
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;
            CurrentSession.StartPercent = 100 * state.LoadedLevel.StartPosition / state.LoadedLevel.PhysicalLength;
            OnSessionInitialized?.Invoke(CurrentSession);
        }

        public void OnSessionEnded(GameState state)
        {
            if (CurrentSession == null || CurrentSession.Attempts.Count == 0) return;
            CurrentSession.Duration = DateTime.Now - CurrentSession.StartTime;
            SessionManager.Instance.SaveSession(CurrentSession);
            OnQuitSession?.Invoke(CurrentSession);
            CurrentSession = null;
            CurrentAttempt = null;
            _isCurrentAttemptSaved = true;
        }

        public void OnAttemptStarted(GameState state)
        {
            CurrentAttempt = new Attempt(state.LoadedLevel.AttemptNumber);
            _isCurrentAttemptSaved = false;
        }

        public void OnAttemptEnded(GameState state)
        {
            if (state == null || state.PlayerObject == null) return;
            float endPercent = 0;
            if (state.PlayerObject.HasWon)
            {
                endPercent = 100;
            } else
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
            CurrentSession = new Session(DateTime.Now);

            if (state == null || state.LevelMetadata == null || state.LoadedLevel == null) return;
            CurrentSession.Level = new Level(state); // TODO Duplicate code
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;
            CurrentSession.StartPercent = 100 * state.LoadedLevel.StartPosition / state.LoadedLevel.PhysicalLength;
            OnSessionInitialized?.Invoke(CurrentSession);
        }

        //Creates an attempt if there is no current attempt and initialize known values
        private void CreateAttemptIfNotExists(GameState state)
        {
            if (CurrentAttempt != null) return;
            CurrentAttempt = new Attempt(state.LoadedLevel.AttemptNumber);
            _isCurrentAttemptSaved = false;

            if (state.LoadedLevel == null) return;
            CurrentAttempt.Number = state.LoadedLevel.AttemptNumber;
        }
    }
}
