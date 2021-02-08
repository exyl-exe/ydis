using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void CreateNewAttempt(GameState state)
        {
            var number = state.LoadedLevel.AttemptNumber;
            var start = state.LoadedLevel.PracticeStartPosition;
            CurrentAttempt = new PracticeAttempt(number, start);
            _isCurrentAttemptSaved = false;
        }

        public void CreateNewSession(GDLevelMetadata level)
        {
            CurrentSession = new PracticeSession(DateTime.Now);
            CurrentAttempt = null;
            _isCurrentAttemptSaved = false;
        }

        public void PopSaveCurrentSession(GameState state)
        {
            if (state != null)
            {
                SilentPopSaveLosingAttempt(state);
            }
            //Don't save if :
            //  -no session were created (= software launched while playing a level, and no attempts have been played before exiting)
            //  -The current level is unknown (= The level was left before it finished loading)
            //  -There are not attempts in the session (= useless data)
            if (CurrentSession == null || CurrentSession.Level == null || CurrentSession.Attempts.Count == 0) return;
            CurrentSession.Duration = DateTime.Now - CurrentSession.StartTime;

            Console.WriteLine("###########");
            Console.WriteLine(CurrentSession.Level.Name);
            foreach(var a in CurrentSession.Attempts)
            {
                Console.WriteLine(string.Format("{0} {1:F2}->{2:F2}", a.Number, a.StartPercent, a.EndPercent));
            }

            CurrentSession = null;
            CurrentAttempt = null;
            _isCurrentAttemptSaved = true;
        }

        public void SilentPopSaveLosingAttempt(GameState state)
        {
            var endPercent = 100 * state.PlayerObject.XPosition / state.LoadedLevel.PhysicalLength;
            PopSaveCurrentAttempt(state, endPercent, true);
        }

        public void PopSaveLosingAttempt(GameState state)
        {
            var endPercent = 100 * state.PlayerObject.XPosition / state.LoadedLevel.PhysicalLength;
            PopSaveCurrentAttempt(state, endPercent);
        }

        public void PopSaveWinningAttempt(GameState state)
        {
            var endPercent = 100;
            PopSaveCurrentAttempt(state, endPercent);
        }

        public void UpdateSessionOnLoad(GameState state)
        {
            CreateSessionIfNotExists(state);
            UpdateSession(state);
        }

        //Updates values of the current session
        private void UpdateSession(GameState state)
        {
            CurrentSession.Level = new Level(state);
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;
            //OnSessionInitialized?.Invoke(CurrentSession);
        }

        // Saves the current attempt with the specified end percent
        private void PopSaveCurrentAttempt(GameState state, float endPercent, bool silent = false)
        {
            if (!state.LoadedLevel.IsPractice && !_isCurrentAttemptSaved)
            {
                CreateSessionIfNotExists(state);
                CreateAttemptIfNotExists(state);
                CurrentAttempt.EndPercent = endPercent;
                CurrentSession.AddAttempt(CurrentAttempt);
                CurrentSession.Duration = DateTime.Now - CurrentSession.StartTime; //Updating duration for UI
                if (!silent)
                {
                    //OnAttemptsUpdated?.Invoke();
                }
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
            UpdateSession(state);
        }

        //Creates an attempt if there is no current attempt and initialize known values
        private void CreateAttemptIfNotExists(GameState state)
        {
            if (CurrentAttempt != null) return;
            var number = state.LoadedLevel.AttemptNumber;
            var start = state.LoadedLevel.PracticeStartPosition;
            CurrentAttempt = new PracticeAttempt(number, start);
            _isCurrentAttemptSaved = false;

            if (state.LoadedLevel == null) return;
            CurrentAttempt.Number = state.LoadedLevel.AttemptNumber;
        }
    }
}
