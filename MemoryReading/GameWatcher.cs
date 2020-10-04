using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Whydoisuck.MemoryReading
{
    static class GameWatcher
    {
        private static int Delay { get; set; } = 10;//ms
        private static bool IsRecording { get; set; }
        private static GDMemoryReader Reader { get; set; } = new GDMemoryReader();
        private static GameState PreviousState { get; set; }
        private static AttemptState CurrentAttempt { get; set; }


        public delegate void GameInfoCallback(GameState gameState);
        public delegate void LevelInfoCallback(GDLevelMetadata level);

        public static event LevelInfoCallback OnLevelEntered;
        public static event LevelInfoCallback OnLevelExited;
        public static event GameInfoCallback OnLevelStarted;

        public static event GameInfoCallback OnPlayerWins;
        public static event GameInfoCallback OnPlayerDies;
        public static event GameInfoCallback OnPlayerRestarts;
        public static event GameInfoCallback OnPlayerSpawns;

        public static void StartWatching()
        {
            if (IsRecording) return;//Can't create more than 1 thread
            IsRecording = true;
            var thread = new Thread(new ThreadStart(UpdateThread));
            thread.Start();
        }

        private static void UpdateThread()
        {
            while (IsRecording)
            {
                Thread.Sleep(Delay);
                UpdateState();
            }
        }

        public static void StopWatching()
        {
            IsRecording = false;
        }

        private static void UpdateState()
        {
            if (!Reader.IsGDOpened)
            {
                var initialized = Reader.TryAttachToGD();
                if (!initialized) return;
            }

            if (PreviousState == null)
            {
                PreviousState = Reader.GetGameState();
                return;
            }

            var currentState = Reader.GetGameState();
            if (currentState == null) return;
            //I'll consider that the refresh frequency is high enough so that it's impossible to switch levels
            //without this function detecting the level was exited (and more generally, impossible to miss any of the relevant game states)
            //Not very clean, but a proper implementation not relying on update frequency seems very difficult to do
            //without injecting code

            HandleLevelExited(PreviousState, currentState);
            HandleLevelEntered(PreviousState, currentState);
            HandleLevelStarted(PreviousState, currentState);
            HandleLevelNotExited(PreviousState, currentState);//TODO not a good idea ?
            PreviousState = currentState;
        }

        private static void HandleLevelExited(GameState previousState, GameState currentState)
        {
            if (currentState.LevelMetadata == null && previousState.LevelMetadata != null)
            {
                OnLevelExited?.Invoke(previousState.LevelMetadata);
            }
        }

        private static void HandleLevelEntered(GameState previousState, GameState currentState)
        {
            if (currentState.LevelMetadata != null && previousState.LevelMetadata == null)
            {
                OnLevelEntered?.Invoke(currentState.LevelMetadata);
            }
        }

        private static void HandleLevelStarted(GameState previousState, GameState currentState)
        {
            if (currentState.LoadedLevel != null)
            {
                if (currentState.LoadedLevel.IsRunning && (previousState.LoadedLevel == null || !previousState.LoadedLevel.IsRunning))
                {
                    OnLevelStarted?.Invoke(currentState);
                    CurrentAttempt = new AttemptState(currentState.LoadedLevel.AttemptNumber);
                }
            }
        }

        private static void HandleLevelNotExited(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject != null && previousState.PlayerObject != null && currentState.LoadedLevel.IsRunning)
            {
                HandlePlayerRestarts(previousState, currentState);
                HandlePlayerDeath(currentState);
                HandlePlayerWin(previousState, currentState);
                HandleRespawn(currentState);
            }
        }

        private static void HandleRespawn(GameState currentState)
        {
            if (currentState.LoadedLevel.AttemptNumber != CurrentAttempt.Number ||//Classic respawn
                CurrentAttempt.HasWon && !currentState.PlayerObject.HasWon)//TODO attempt 1
            {
                OnPlayerSpawns?.Invoke(currentState);
                CurrentAttempt = new AttemptState(currentState.LoadedLevel.AttemptNumber);
            }
        }

        private static void HandlePlayerRestarts(GameState previousState, GameState currentState)//Manual restart
        {
            if (currentState.LoadedLevel.AttemptNumber != CurrentAttempt.Number && !CurrentAttempt.HasEnded) //TODO would be better to use position ?
            {
                OnPlayerRestarts?.Invoke(previousState);
            }
        }

        private static void HandlePlayerWin(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject.HasWon && !previousState.PlayerObject.HasWon)
            {
                CurrentAttempt.HasWon = true;
                OnPlayerWins?.Invoke(currentState);
            }
        }

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
    }

    class AttemptState
    {
        public AttemptState(int number)
        {
            Number = number;
            HasDied = false;
            HasWon = false;
        }

        public int Number { get; private set; }
        public bool HasDied { get; set; }
        public bool HasWon { get; set; }
        public bool HasEnded { get { return HasWon || HasDied; } }
    }
}
