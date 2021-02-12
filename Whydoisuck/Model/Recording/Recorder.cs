using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.Model.DataSaving;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.MemoryReading;
using Whydoisuck.Model.MemoryReading.GameStateStructures;

namespace Whydoisuck.Model.Recording
{
    /// <summary>
    /// Saves data on the disk based on what happens in the game
    /// </summary>
    public class Recorder
    {
        public event Action<ISession> OnStartSession;
        public event Action<ISession> OnQuitSession;
        public event Action OnSessionAttemptsUpdated;

        /// <summary>
        /// Guessed group for the current session.
        /// </summary>
        public SessionGroup Autoguess => _currentState!=null?_currentState.Autoguess:null;

        // Current state of the recorder
        private IRecorderState _currentState;

        public Recorder()
        {
            GameWatcher.OnLevelEntered += LevelEntered;
            GameWatcher.OnLevelStarted += SessionStarted;
            GameWatcher.OnLevelExited += SessionEnded;
            GameWatcher.OnPlayerSpawns += AttemptStarted;
            GameWatcher.OnPlayerDies += AttemptEnded;
            GameWatcher.OnPlayerRestarts += AttemptEnded;
            GameWatcher.OnPlayerWins += AttemptEnded;
            GameWatcher.OnPracticeModeStarted += SetPracticeModeState;
            GameWatcher.OnPracticeModeExited += QuitPracticeModeState;
            GameWatcher.OnNormalModeStarted += SetNewNormalModeState;
            GameWatcher.OnNormalModeExited += QuitNormalModeState;
            GameWatcher.OnGameClosed += () => SessionEnded(null);
        }

        /// <summary>
        /// Starts saving data based on what happens in game.
        /// </summary>
        public void StartRecording()
        {
            GameWatcher.StartWatching();
        }

        /// <summary>
        /// Stops saving data and saves the on going session
        /// </summary>
        public void StopRecording()
        {
            GameWatcher.StopWatching();
            SessionEnded(null);
            SessionManager.Instance.Save();
        }

        /// <summary>
        /// Prevent the recorder from saving data.
        /// </summary>
        public void CrashRecorder()
        {
            //TODO prevent callbacks from being called
            GameWatcher.CancelWatchingAsync();
        }

        private void LevelEntered(GDLevelMetadata level)
        {
            SetState(new NormalRecorderState());
        }

        private void SessionStarted(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState.OnSessionStarted(state);
        }

        private void SessionEnded(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState.OnSessionEnded(state);
        }

        private void AttemptStarted(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState.OnAttemptStarted(state);
        }

        private void AttemptEnded(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState.OnAttemptEnded(state);
        }

        // Makes the recorder record a normal mode session after a practice session
        private void SetNewNormalModeState(GameState state)
        {
            SetState(new NormalRecorderState());
            SessionStarted(state);
        }

        // Called when normal mode is exited (in order to save current session)
        private void QuitNormalModeState(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            SessionEnded(state);
            SetState(null);
        }

        // Makes the recorder record a practice mode session
        private void SetPracticeModeState(GameState state)
        {
            SetState(new PracticeRecorderState());
            SessionStarted(state);
        }

        private void QuitPracticeModeState(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            SessionEnded(state);
            SetState(null);
        }

        // Sets the recorder state
        private void SetState(IRecorderState s)
        {
            if (_currentState != null)
            {
                _currentState.OnAttemptsUpdated -= OnStateAttemptsUpdated;
                _currentState.OnSessionInitialized -= OnStateNewSession;
                _currentState.OnQuitSession -= OnStateQuitSession;
            }
            if (s != null)
            {
                s.OnAttemptsUpdated += OnStateAttemptsUpdated;
                s.OnSessionInitialized += OnStateNewSession;
                s.OnQuitSession += OnStateQuitSession;
            }
            _currentState = s;
        }

        // Used to init the current state if the recorder was started while playing a level
        private void InitRecorderStateIfNeeded(GameState gameState)
        {
            if (gameState == null || gameState.LoadedLevel == null) return;
            if (_currentState == null)
            {
                if (!gameState.LoadedLevel.IsPractice)
                {
                    SetNewNormalModeState(gameState);
                }
                else
                {
                    SetPracticeModeState(gameState);
                }
            }
        }

        // Handles the current state ending a session
        private void OnStateQuitSession(ISession obj)
        {
            OnQuitSession?.Invoke(obj);
        }

        // Handles the current state creating a session
        private void OnStateNewSession(ISession obj)
        {
            OnStartSession?.Invoke(obj);
        }

        // Handles the current state updating it's current session attempts
        private void OnStateAttemptsUpdated()
        {
            OnSessionAttemptsUpdated?.Invoke();
        }
    }
}
