using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.Model.MemoryReading.GameStateStructures;
using Whydoisuck.Model.UserSettings;
using Whydoisuck.Properties;

namespace Whydoisuck.Model.MemoryReading
{
    /// <summary>
    /// Class <c>GameWatcher</c> has several events invoked based on what happens in the game
    /// </summary>
    public static class GameWatcher
    {
        /// <summary>
        /// Delegate for events providing new values about the game's state
        /// </summary>
        public delegate void GameInfoCallback(GameState gameState);
        /// <summary>
        /// Delegate for events providing new values about the current level
        /// </summary>
        public delegate void LevelInfoCallback(GDLevelMetadata level);
        /// <summary>
        /// Invoked when the game is closed
        /// </summary>
        public static event Action OnGameClosed;
        /// <summary>
        /// Event invoked when a level is entered in-game
        /// </summary>
        public static event LevelInfoCallback OnLevelEntered;
        /// <summary>
        /// Event invoked when a level is exited in-game
        /// </summary>
        public static event GameInfoCallback OnLevelExited;
        /// <summary>
        /// Event invoked when the current level has finished loading.
        /// It ensures its length, the player object and some other values are initialized.
        /// </summary>
        public static event GameInfoCallback OnLevelStarted;

        /// <summary>
        /// Event invoked when the player object reached the end of the level
        /// </summary>
        public static event GameInfoCallback OnPlayerWins;
        /// <summary>
        /// Event invoked when the player object dies
        /// </summary>
        public static event GameInfoCallback OnPlayerDies;
        /// <summary>
        /// Event invoked when the user has manually restarted an attempt
        /// </summary>
        public static event GameInfoCallback OnPlayerRestarts;
        /// <summary>
        /// Event invoked when the player object just respawned
        /// </summary>
        public static event GameInfoCallback OnPlayerSpawns;

        // Delay between every check of the game's state
        private static int Delay => WDISSettings.ScanPeriod;//milliseconds
        // Delay between every attach attempt
        private static int AttachDelay => WDISSettings.AttachPeriod;
        // Boolean true if the GameWatcher is active
        private static bool IsRecording { get; set; }
        // Reader to access the game's memory
        private static GDMemoryReader Reader { get; set; } = new GDMemoryReader();
        // Previously read state
        private static GameState PreviousState { get; set; }
        // Current on going attempt
        private static WatchedAttempt CurrentAttempt { get; set; }
        // Current thread that reads the game's memory and manages events.
        private static Thread CurrentWatchThread { get; set; }

        /// <summary>
        /// Called to start watching the game's process.
        /// Events will be invoked based on what happens in the game.
        /// </summary>
        public static void StartWatching()
        {
            if (IsRecording) return;//Can't create more than 1 thread
            IsRecording = true;
            PreviousState = null;
            CurrentAttempt = null;
            CurrentWatchThread = new Thread(new ThreadStart(UpdateThread));
            CurrentWatchThread.Start();
        }

        // Loop to update the game's state
        private static void UpdateThread()
        {
            while (IsRecording)
            {
                // Attaches the reader to the process before reading memory values
                if (Reader.IsGDOpened || Reader.TryAttachToGD())
                {
                    while (Reader.IsGDOpened && IsRecording)
                    {
                        UpdateState();
                        Thread.Sleep(Delay);
                    }
                    if (!Reader.IsGDOpened && IsRecording)
                    {
                        OnGameClosed?.Invoke();
                    }
                }                
                Thread.Sleep(AttachDelay);
            }
        }

        /// <summary>
        /// Called to stop watching the game's process.
        /// </summary>
        public static void StopWatching()
        {
            IsRecording = false;
            CurrentWatchThread.Join();
        }

        /// <summary>
        /// Called to stop watching the game's process. Returns before the gamewatcher actually stops.
        /// </summary>
        public static void CancelWatchingAsync()
        {
            IsRecording = false;
        }

        // Manages events based on the state of the game
        private static void UpdateState()
        {
            // First time reading the game's state
            if (PreviousState == null)
            {
                PreviousState = Reader.GetGameState();
                return;
            }

            var currentState = Reader.GetGameState();
            if (currentState == null) return;
            HandleLevelExited(PreviousState, currentState);
            HandleLevelEntered(PreviousState, currentState);
            HandleLevelStarted(PreviousState, currentState);
            HandleLevelNotExited(PreviousState, currentState);
            PreviousState = currentState;
        }

        // Compares two successive states of the game,
        // in order to invoke the OnLevelExited event if needed.
        private static void HandleLevelExited(GameState previousState, GameState currentState)
        {
            if(previousState.PlayerObject!=null && currentState.PlayerObject == null)
            {
                OnLevelExited?.Invoke(previousState);
            }
        }

        // Compares two successive states of the game,
        // in order to invoke the OnLevelEntered event if needed.
        private static void HandleLevelEntered(GameState previousState, GameState currentState)
        {
            if (currentState.LevelMetadata != null && previousState.LevelMetadata == null)
            {
                OnLevelEntered?.Invoke(currentState.LevelMetadata);
            }
        }

        // Compares two successive states of the game,
        // in order to invoke the OnLevelStarted event if needed.
        private static void HandleLevelStarted(GameState previousState, GameState currentState)
        {
            if (currentState.LoadedLevel != null)
            {
                if (currentState.LoadedLevel.IsRunning && (previousState.LoadedLevel == null || !previousState.LoadedLevel.IsRunning))
                {
                    OnLevelStarted?.Invoke(currentState);
                    CurrentAttempt = new WatchedAttempt(currentState.LoadedLevel.AttemptNumber);
                }
            }
        }

        // Manages events about an on going level
        private static void HandleLevelNotExited(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject != null && previousState.PlayerObject != null && currentState.LoadedLevel.IsRunning)
            {
                //Needed because the game watcher might have been started while playing a level
                if (CurrentAttempt == null)
                {
                    CurrentAttempt = new WatchedAttempt(
                        currentState.LoadedLevel.AttemptNumber,
                        currentState.PlayerObject.IsDead,
                        currentState.PlayerObject.HasWon
                        );
                }
                HandlePlayerRestarts(previousState, currentState);
                HandlePlayerDeath(currentState);
                HandlePlayerWin(previousState, currentState);
                HandleRespawn(currentState);
            }
        }

        // Checks the current state of the game,
        // in order to invoke the OnPlayerSpawns event if needed.
        private static void HandleRespawn(GameState currentState)
        {
            if (currentState.LoadedLevel.AttemptNumber != CurrentAttempt.Number ||//Classic respawn
                CurrentAttempt.HasWon && !currentState.PlayerObject.HasWon)//Respawn after winning on first attempt
            {
                 OnPlayerSpawns?.Invoke(currentState);
                CurrentAttempt = new WatchedAttempt(currentState.LoadedLevel.AttemptNumber);
            }
        }

        // Checks the current state of the game,
        // in order to invoke the OnPlayerRestarts event if needed.
        private static void HandlePlayerRestarts(GameState previousState, GameState currentState)//Manual restart
        {
            if (currentState.LoadedLevel.AttemptNumber != CurrentAttempt.Number && !CurrentAttempt.HasEnded)
            {
                OnPlayerRestarts?.Invoke(previousState);
            }
        }

        // Checks the current state of the game,
        // in order to invoke the OnPlayerWins event if needed.
        private static void HandlePlayerWin(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject.HasWon && !previousState.PlayerObject.HasWon)
            {
                CurrentAttempt.HasWon = true;
                OnPlayerWins?.Invoke(currentState);
            }
        }

        // Checks the current state of the game,
        // in order to invoke the OnPlayerDies event if needed.
        private static void HandlePlayerDeath(GameState currentState)
        {
            if (currentState.PlayerObject.IsDead)
            {
                if (!CurrentAttempt.HasDied)
                //Most common case, player went from alive to dead
                {
                    OnPlayerDies?.Invoke(currentState);
                }

                if (CurrentAttempt.Number != currentState.LoadedLevel.AttemptNumber)
                //Dead and previously not same attempt, the player died on the first frame after respawning
                {
                    OnPlayerDies?.Invoke(currentState);
                }

                CurrentAttempt.HasDied = true;
            }
        }

        // Private class to store data about the on going attempt
        private class WatchedAttempt
        {
            public WatchedAttempt(int number, bool died, bool won)
            {
                Number = number;
                HasDied = died;
                HasWon = won;
            }

            public WatchedAttempt(int number) : this(number, false, false){}

            public int Number { get; private set; }
            public bool HasDied { get; set; }
            public bool HasWon { get; set; }
            public bool HasEnded { get { return HasWon || HasDied; } }
        }
    }
}
