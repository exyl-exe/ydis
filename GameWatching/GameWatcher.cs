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
        private int Delay { get; set; } = 100;//ms
        private bool IsRecording { get; set; }
        private GDMemoryReader Reader { get; set; }
        private GameState previousState { get; set; }

        public delegate void GameInfoCallback(GameState gameState);
        public delegate void LevelInfoCallback(GDLoadedLevelInfos level);
        public delegate void PlayerInfoCallback(GDPlayerInfos player);

        public event GameInfoCallback OnGameExited;
        public event LevelInfoCallback OnLevelEntered;
        public event LevelInfoCallback OnLevelExited;
        public event LevelInfoCallback OnNewAttempt;
        public event PlayerInfoCallback OnPlayerObjectCreated;
        public event PlayerInfoCallback OnPlayerObjectDestroyed;
        public event PlayerInfoCallback OnPlayerWins;
        public event PlayerInfoCallback OnPlayerDies;
        public event PlayerInfoCallback OnPlayerSpawns;

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
                if (previousState != null)
                {
                    OnGameExited.Invoke(previousState);
                    previousState = null;
                }
                var initialized = Reader.TryAttachToGD();
                if (!initialized) return;
            }

            if (previousState == null)
            {
                previousState = Reader.GetGameState();
                return;
            }

            var currentState = Reader.GetGameState();
            //I'll consider that the refresh frequency is high enough so that it's impossible to switch levels
            //without this function detecting the level was exited (and more generally, impossible to miss any of the relevant game states)
            //Not very clean, but a proper implementation not relying on update frequency seems very difficult to do

            //Level unloaded
            if (currentState.PlayedLevel == null && previousState.PlayedLevel != null)
            {
                OnLevelExited.Invoke(previousState.PlayedLevel);
            }

            //Level loaded, no info on player object
            if (currentState.PlayedLevel != null && previousState.PlayedLevel == null)
            {
                OnLevelEntered.Invoke(currentState.PlayedLevel);
                OnNewAttempt.Invoke(currentState.PlayedLevel);
            }

            //Player object created
            if (currentState.PlayerObject == null && previousState.PlayerObject != null)
            {
                OnPlayerObjectCreated.Invoke(currentState.PlayerObject);
                OnPlayerSpawns.Invoke(currentState.PlayerObject);
            }

            //Player object destroyed
            if (currentState.PlayerObject != null && previousState.PlayerObject == null)
            {
                OnPlayerObjectDestroyed.Invoke(previousState.PlayerObject);
            }

            //Level was not exited
            if (currentState.PlayedLevel != null && previousState.PlayedLevel != null)
            {
                //Attempt count has changed
                if (currentState.PlayedLevel.AttemptNumber != previousState.PlayedLevel.AttemptNumber)
                {
                    OnNewAttempt.Invoke(currentState.PlayedLevel);
                    OnPlayerSpawns.Invoke(currentState.PlayerObject);
                }
            }

            //Level was not exited and player object is fully loaded
            if (currentState.PlayerObject != null && previousState.PlayerObject != null)
            {
                if(currentState.PlayerObject.HasWon && !previousState.PlayerObject.HasWon)
                {
                    OnPlayerWins.Invoke(currentState.PlayerObject);
                }

                if (currentState.PlayerObject.IsDead && !previousState.PlayerObject.IsDead)
                {
                    OnPlayerDies.Invoke(currentState.PlayerObject);
                }
            }

            previousState = currentState;
        }
    }
}
