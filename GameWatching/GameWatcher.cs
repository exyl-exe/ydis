using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;

namespace Whydoisuck.GameWatching
{
    static class GameWatcher
    {
        private static int Delay { get; set; } = 10;//ms
        private static bool IsRecording { get; set; }
        private static GDMemoryReader Reader { get; set; } = new GDMemoryReader();
        private static GameState PreviousState { get; set; }

        public delegate void GameInfoCallback(GameState gameState);
        public delegate void LevelInfoCallback(GDLoadedLevelInfos level);

        public static event LevelInfoCallback OnLevelEntered;
        public static event LevelInfoCallback OnLevelExited;
        public static event LevelInfoCallback OnNewAttempt;
        public static event GameInfoCallback OnPlayerObjectCreated;
        public static event GameInfoCallback OnPlayerObjectDestroyed;
        public static event GameInfoCallback OnPlayerWins;
        public static event GameInfoCallback OnPlayerDies;
        public static event GameInfoCallback OnPlayerSpawns;

        public static void UpdateThread()
        {
            while (IsRecording)
            {
                Thread.Sleep(Delay);
                UpdateState();
            }
        }

        public static void StartWatching()
        {
            if (IsRecording) return;//Can't create more than 1 thread
            IsRecording = true;
            var thread = new Thread(new ThreadStart(UpdateThread));
            thread.Start();
        }

        public static void StopWatching()
        {
            IsRecording = false;
        }

        public static void UpdateState()
        {
            if (!Reader.IsGDOpened){
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
            //without modifying the game's code
            HandleLevelUnloaded(PreviousState, currentState);
            HandleLevelLoaded(PreviousState, currentState);
            HandlePlayerObjectCreated(PreviousState, currentState);
            HandlePlayerObjectDestroyed(PreviousState, currentState);
            HandleLevelNotExited(PreviousState, currentState);
            PreviousState = currentState;
        }

        public static void HandleLevelUnloaded(GameState previousState, GameState currentState)
        {
            if (currentState.PlayedLevel == null && previousState.PlayedLevel != null)
            {
                OnLevelExited?.Invoke(previousState.PlayedLevel);
            }
        }

        public static void HandleLevelLoaded(GameState previousState, GameState currentState)
        {
            if (currentState.PlayedLevel != null && previousState.PlayedLevel == null)
            {
                OnLevelEntered?.Invoke(currentState.PlayedLevel);
                OnNewAttempt?.Invoke(currentState.PlayedLevel);
            }
        }

        private static void HandlePlayerObjectCreated(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject != null && previousState.PlayerObject == null)
            {
                if (currentState.PlayedLevel == null)
                {
                    TempLogger.AddLog("Played level null");
                    return;
                }
                OnPlayerObjectCreated?.Invoke(currentState);
                OnPlayerSpawns?.Invoke(currentState);
            }
        }

        private static void HandlePlayerObjectDestroyed(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject == null && previousState.PlayerObject != null)
            {
                OnPlayerObjectDestroyed?.Invoke(previousState);
            }
        }

        private static void HandleLevelNotExited(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject != null && previousState.PlayerObject != null)
            {
                HandleRespawn(previousState, currentState);
                HandlePlayerDeath(previousState, currentState);
                HandlePlayerWin(previousState, currentState);
            }
        }

        private static void HandleRespawn(GameState previousState, GameState currentState)
        { 
            if (currentState.PlayedLevel.AttemptNumber != previousState.PlayedLevel.AttemptNumber ||//Classic respawn
                previousState.PlayerObject.HasWon && !currentState.PlayerObject.HasWon)//Attempt winning respawn
            {
                OnNewAttempt?.Invoke(currentState.PlayedLevel);
                OnPlayerSpawns?.Invoke(currentState);
            }
        }

        private static void HandlePlayerWin(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject.HasWon && !previousState.PlayerObject.HasWon)
            {
                OnPlayerWins?.Invoke(currentState);
            }
        }

        private static void HandlePlayerDeath(GameState previousState, GameState currentState)
        {
            //second part of the condition prevents being screwed by instant death
            if (currentState.PlayerObject.IsDead && (!previousState.PlayerObject.IsDead || previousState.PlayedLevel.AttemptNumber != currentState.PlayedLevel.AttemptNumber))
            {
                OnPlayerDies?.Invoke(currentState);
            }
        }
    }
}
