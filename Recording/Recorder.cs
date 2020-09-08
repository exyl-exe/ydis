using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.GameWatching;
using Whydoisuck.MemoryReading;

namespace Whydoisuck.Recording
{
    class Recorder
    {
        private GameWatcher gameWatcher { get; set; }

        private Session CurrentSession { get; set; }
        private Attempt CurrentAttempt { get; set; }


        public Recorder()
        {
            gameWatcher = new GameWatcher();
            //TODO minimalize the impact of the order in which events are fired
            gameWatcher.OnPlayerObjectCreated += CreateNewSession;
            gameWatcher.OnLevelExited += SaveCurrentSession;
            gameWatcher.OnPlayerSpawns += CreateNewAttempt;
            gameWatcher.OnPlayerDies += SaveCurrentAttempt;
            gameWatcher.OnPlayerWins += SaveCurrentWinningAttempt;
        }

        public void StartRecording()
        {
            gameWatcher.StartWatching();
        }

        public void StopRecording()
        {
            gameWatcher.StopWatching();
        }

        public void SaveCurrentAttempt(GameState state)
        {
            if (CurrentSession == null)
            {
                CreateNewSession(state);
                return;
            }
            CurrentAttempt.EndPercent = 100 * state.PlayerObject.XPosition / state.PlayedLevel.Length;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        public void SaveCurrentWinningAttempt(GameState state)
        {
            if (CurrentSession == null)
            {
                CreateNewSession(state);
                return;
            }
            CurrentAttempt.EndPercent = 100;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        public void CreateNewSession(GameState state)
        {
            CurrentSession = new Session
            {
                LevelID = state.PlayedLevel.ID,
                Attempts = new List<Attempt>(),
                startTime = DateTime.Now,
                LevelName = state.PlayedLevel.Name
            };
        }

        public void SaveCurrentSession(GameState state)
        {
            SaveCurrentSession();
        }

        public void SaveCurrentSession(GDLoadedLevelInfos state)
        {
            SaveCurrentSession();
        }

        public void SaveCurrentSession()
        {
            if (CurrentSession == null) return;

            //TODO save to json
            var session = $"[{CurrentSession.LevelName}] {CurrentSession.startTime}, {CurrentSession.Attempts.Count} attempts";
            foreach (var att in CurrentSession.Attempts)
            {
                session += $"\n\t Attempt {att.Number}, {(int)att.StartPercent}%-{(int)att.EndPercent}%, {att.Duration.TotalSeconds}s";
            }
            TempLogger.AddLog(session);
        }

        public void CreateNewAttempt(GameState state)
        {
            CurrentAttempt = new Attempt()
            {
                StartTime = DateTime.Now,
                Number = state.PlayedLevel.AttemptNumber,
                StartPercent = 100 * state.PlayerObject.XPosition / state.PlayedLevel.Length
            };
        }
    }
}
