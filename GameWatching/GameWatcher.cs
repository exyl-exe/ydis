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
        public delegate void LevelInfoCallback(GDLevelMetadata level);

        public static event LevelInfoCallback OnLevelEntered;
        public static event LevelInfoCallback OnLevelExited;
        public static event GameInfoCallback OnLevelStarted;

        /*public static event GameInfoCallback OnPlayerObjectCreated;
        public static event GameInfoCallback OnPlayerObjectDestroyed;*///TODO remove ?
        public static event GameInfoCallback OnPlayerWins;
        public static event GameInfoCallback OnPlayerDies;
        public static event GameInfoCallback OnPlayerRestarts;
        public static event GameInfoCallback OnPlayerSpawns;

        /*
         Cycle of events :
         OnLevelEntered
         OnLevelStarted
         loop:
            OnPlayerSpawns
            OnPlayerWins || OnPlayersDies
         OnLevelExited
         */

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
            //without injecting code
            
            HandleLevelExited(PreviousState, currentState);
            HandleLevelEntered(PreviousState, currentState);
            HandleLevelStarted(PreviousState, currentState);
            HandleLevelNotExited(PreviousState, currentState);//TODO not a good idea ?
            /*HandlePlayerObjectCreated(PreviousState, currentState);//TODO Remove ?
HandlePlayerObjectDestroyed(PreviousState, currentState);*/
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
                if(currentState.LoadedLevel.IsRunning && (previousState.LoadedLevel == null || !previousState.LoadedLevel.IsRunning))
                {
                    OnLevelStarted?.Invoke(currentState);
                }
            }
        }

        //TODO remove ?
        /*private static void HandlePlayerObjectCreated(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject != null && previousState.PlayerObject == null)
            {
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
        }*/

        private static void HandleLevelNotExited(GameState previousState, GameState currentState)
        {
            if (currentState.PlayerObject != null && previousState.PlayerObject != null)
            {
                HandlePlayerRestarts(previousState, currentState);
                HandlePlayerDeath(previousState, currentState);
                HandlePlayerWin(previousState, currentState);
                HandleRespawn(previousState, currentState);
            }
        }

        private static void HandleRespawn(GameState previousState, GameState currentState)
        { 
            if (currentState.LoadedLevel.AttemptNumber != previousState.LoadedLevel.AttemptNumber ||//Classic respawn
                previousState.PlayerObject.HasWon && !currentState.PlayerObject.HasWon)//Attempt winning respawn
            {
                OnPlayerSpawns?.Invoke(currentState);
            }
        }

        private static void HandlePlayerRestarts(GameState previousState, GameState currentState)//Manual restart
        {
            if (currentState.LoadedLevel.AttemptNumber != previousState.LoadedLevel.AttemptNumber && !previousState.PlayerObject.HasWon && !previousState.PlayerObject.IsDead)
            {
                OnPlayerRestarts?.Invoke(currentState);
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
            if (currentState.PlayerObject.IsDead &&//player is dead
                (!previousState.PlayerObject.IsDead//previously not dead
                || previousState.PlayerObject.HasWon//previously had won = instant death on respawn after win (how is that possible)
                || (previousState.LoadedLevel.AttemptNumber != currentState.LoadedLevel.AttemptNumber)))//previously not same attempt = instant death on respawn
            {
                OnPlayerDies?.Invoke(currentState);
            }
        }
    }
}
