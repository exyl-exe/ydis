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
        public event Action<ISession> OnSessionInitialized;
        public event Action<ISession> OnQuitSession;
        public event Action OnAttemptsUpdated;

        private PracticeSession CurrentSession { get; set; }
        private bool _isCurrentAttemptSaved;
        private PracticeAttempt CurrentAttempt { get; set; }

        public void OnSessionStarted(GameState state)
        {
            if (state == null || state.LoadedLevel == null || !state.LoadedLevel.IsRunning) return;

            CurrentSession = new PracticeSession(state, DateTime.Now);
            CreateInitialAttempt(state);
            _isCurrentAttemptSaved = false;
            OnSessionInitialized?.Invoke(CurrentSession);
        }

        public void OnSessionEnded(GameState state)
        {
            if (CurrentSession == null || CurrentSession.Attempts.Count == 0) return;
            CurrentSession.Duration = DateTime.Now - CurrentSession.StartTime;
            SessionManager.Instance.SavePracticeSession(CurrentSession);
            OnQuitSession?.Invoke(CurrentSession); // TODO above condition ? see session creation conditions
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
            if (state == null || !state.LoadedLevel.IsRunning) return;
            float endPercent = GetEndPercent(state);
            SaveCurrentAttempt(state, endPercent);
        }

        // Returns what is the end percent of an attempt that died
        private float GetEndPercent(GameState state)
        {
            if (state.PlayerObject.HasWon)
            {
                return 100;
            }
            else
            {
                return 100 * state.PlayerObject.XPosition / state.LoadedLevel.PhysicalLength;
            }
        }

        // Creates the initial attempt of a practice mode session
        private void CreateInitialAttempt(GameState state)
        {
            if (!state.PlayerObject.IsDead)
            {
                var number = state.LoadedLevel.AttemptNumber;
                var firstStartPercent = state.LoadedLevel.StartPosition * 100 / state.LoadedLevel.PhysicalLength;
                CurrentAttempt = new PracticeAttempt(number, firstStartPercent);
            } else
            {
                CurrentAttempt = null;
            }
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
            if (CurrentSession != null || state == null) return;
            CurrentSession = new PracticeSession(state, DateTime.Now);
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
