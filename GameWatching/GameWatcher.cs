using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;

namespace Whydoisuck.GameWatching
{
    class GameWatcher//TODO static class
    {
        private int Delay { get; set; } = 10;//ms
        private bool IsRecording { get; set; }
        private GDMemoryReader Reader { get; set; }
        private GameState previousState { get; set; }

        public delegate void GameInfoCallback(GameState gameState);
        public delegate void LevelInfoCallback(GDLoadedLevelInfos level);

        public event LevelInfoCallback OnLevelEntered;
        public event LevelInfoCallback OnLevelExited;
        public event LevelInfoCallback OnNewAttempt;
        public event GameInfoCallback OnPlayerObjectCreated;
        public event GameInfoCallback OnPlayerObjectDestroyed;
        public event GameInfoCallback OnPlayerWins;
        public event GameInfoCallback OnPlayerDies;
        public event GameInfoCallback OnPlayerSpawns;

        public GameWatcher()
        {
            Reader = new GDMemoryReader();
        }

        public void UpdateThread()
        {
            while (IsRecording)
            {
                Thread.Sleep(Delay);
                UpdateState();
            }
        }

        public void StartWatching()
        {
            IsRecording = true;
            var thread = new Thread(new ThreadStart(UpdateThread));
            thread.Start();
        }

        public void StopWatching()
        {
            IsRecording = false;
        }

        public void UpdateState()
        {
            if (!Reader.IsGDOpened){
                var initialized = Reader.TryAttachToGD();
                if (!initialized) return;
            }

            if (previousState == null)
            {
                previousState = Reader.GetGameState();
                return;
            }

            var currentState = Reader.GetGameState();
            if (currentState == null) return;
            //I'll consider that the refresh frequency is high enough so that it's impossible to switch levels
            //without this function detecting the level was exited (and more generally, impossible to miss any of the relevant game states)
            //Not very clean, but a proper implementation not relying on update frequency seems very difficult to do
            //without modifying the game's code
            HandleLevelUnloaded(previousState, currentState);
            HandleLevelLoaded(previousState, currentState);
            HandlePlayerObjectCreated(previousState, currentState);
            HandlePlayerObjectDestroyed(previousState, currentState);
            HandleLevelNotExited(previousState, currentState);
            previousState = currentState;
        }

        public void HandleLevelUnloaded(GameState previousState, GameState currentState)
        {
            if (currentState.PlayedLevel == null && previousState.PlayedLevel != null)
            {
                OnLevelExited?.Invoke(previousState.PlayedLevel);
            }
        }

        public void HandleLevelLoaded(GameState previousState, GameState currentState)
        {
            if (currentState.PlayedLevel != null && previousState.PlayedLevel == null)
            {
                OnLevelEntered?.Invoke(currentState.PlayedLevel);
                OnNewAttempt?.Invoke(currentState.PlayedLevel);
            }
        }

        private void HandlePlayerObjectCreated(GameState previousState, GameState currentState)
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

        private void HandlePlayerObjectDestroyed(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject == null && previousState.PlayerObject != null)
            {
                OnPlayerObjectDestroyed?.Invoke(previousState);
            }
        }

        private void HandleLevelNotExited(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject != null && previousState.PlayerObject != null)
            {
                HandleRespawn(previousState, currentState);
                HandlePlayerDeath(previousState, currentState);
                HandlePlayerWin(previousState, currentState);
            }
        }

        private void HandleRespawn(GameState previousState, GameState currentState)
        { 
            if (currentState.PlayedLevel.AttemptNumber != previousState.PlayedLevel.AttemptNumber ||//Classic respawn
                previousState.PlayerObject.HasWon && !currentState.PlayerObject.HasWon)//Attempt winning respawn
            {
                OnNewAttempt?.Invoke(currentState.PlayedLevel);
                OnPlayerSpawns?.Invoke(currentState);
            }
        }

        private void HandlePlayerWin(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject.HasWon && !previousState.PlayerObject.HasWon)
            {
                OnPlayerWins?.Invoke(currentState);
            }
        }

        private void HandlePlayerDeath(GameState previousState, GameState currentState)
        {
            //second part of the condition prevents being screwed by instant death
            if (currentState.PlayerObject.IsDead && (!previousState.PlayerObject.IsDead || previousState.PlayedLevel.AttemptNumber != currentState.PlayedLevel.AttemptNumber))
            {
                OnPlayerDies?.Invoke(currentState);
            }
        }
    }
}
