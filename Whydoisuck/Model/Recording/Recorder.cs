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
            GameWatcher.OnGameClosed += () => PopSaveCurrentSession(null);
            GameWatcher.OnLevelEntered += LevelEntered;
            GameWatcher.OnLevelStarted += UpdateSessionOnLoad;
            GameWatcher.OnLevelExited += PopSaveCurrentSession;
            GameWatcher.OnPlayerSpawns += CreateNewAttempt;
            GameWatcher.OnPlayerDies += PopSaveLosingAttempt;
            GameWatcher.OnPlayerRestarts += PopSaveLosingAttempt;
            GameWatcher.OnPlayerWins += PopSaveWinningAttempt;
            GameWatcher.OnPracticeModeStarted += SetPracticeModeState;
            GameWatcher.OnPracticeModeExited += QuitPracticeModeState;
            GameWatcher.OnNormalModeStarted += SetNewNormalModeState;
            GameWatcher.OnNormalModeExited += QuitNormalModeState;
        }

        /// <summary>
        /// Starts saving data based on what happens in game.
        /// </summary>
        public void StartRecording()
        {
            GameWatcher.StartWatching();
        }

        /// <summary>
        /// Stops saving data.
        /// </summary>
        public void StopRecording()
        {
            GameWatcher.StopWatching();
            PopSaveCurrentSession(null);
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

        // Called when entering a level, ensure a session is created before an attempt needs to be saved
        // However, while its metadata is fully loaded, the level is not
        // Therefore stuff like the level length, the start position etc. is updated when the level is fully loaded and not in this function
        private void LevelEntered(GDLevelMetadata level)
        {
            SetState(new NormalRecorderState());
            _currentState?.CreateNewSession(level);
        }

        // Update values for the current session, is called when the level is fully loaded
        private void UpdateSessionOnLoad(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState?.UpdateSessionOnLoad(state);
        }

        //Saves current session and removes it from the recorder. Overload for event with parameters.
        private void PopSaveCurrentSession(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState?.PopSaveCurrentSession(state);
        }

        // Creates an attempt
        private void CreateNewAttempt(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState?.CreateNewAttempt(state);
        }

        // Saves a losing attempt in the current session, and remove current attempt from recorder
        private void PopSaveLosingAttempt(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState?.PopSaveLosingAttempt(state);
        }

        // Saves a winning attempt in the current session, and remove current attempt from recorder
        private void PopSaveWinningAttempt(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState?.PopSaveWinningAttempt(state);
        }

        // Makes the recorder record a normal mode session after a practice session
        private void SetNewNormalModeState(GameState state)
        {
            SetState(new NormalRecorderState());
            _currentState.CreateNewSession(state.LevelMetadata);
            if (state.LoadedLevel.IsRunning)
            {
                _currentState.UpdateSessionOnLoad(state);
            }
        }

        // Called when normal mode is exited (in order to save current session)
        private void QuitNormalModeState(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState.PopSaveCurrentSession(state);
            SetState(null);
        }

        // Makes the recorder record a practice mode session
        private void SetPracticeModeState(GameState state)
        {
            SetState(new PracticeRecorderState());
            _currentState.CreateNewSession(state.LevelMetadata);
            if (state.LoadedLevel.IsRunning)
            {
                _currentState.UpdateSessionOnLoad(state);
            }
        }

        private void QuitPracticeModeState(GameState state)
        {
            InitRecorderStateIfNeeded(state);
            _currentState.PopSaveCurrentSession(state);
            SetState(null);
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
