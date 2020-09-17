using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.GameWatching;
using Whydoisuck.MemoryReading;

namespace Whydoisuck.DataSaving
{
    class Recorder
    {
        private Session CurrentSession { get; set; }
        private Attempt CurrentAttempt { get; set; }

        const float MIN_PERCENT_COPY = 0.5f;//Exists because start pos is determined by the earliest measured player position
        //I hope nobody ever has to put a startpos this early


        public Recorder()
        {
            //TODO minimalize the impact of the order in which events are fired
            //TODO on level settings loaded
            GameWatcher.OnPlayerObjectCreated += CreateNewSession;
            GameWatcher.OnLevelExited += SaveCurrentSession;
            GameWatcher.OnPlayerSpawns += CreateNewAttempt;
            GameWatcher.OnPlayerSpawns += UpdateSessionStartPercent;
            GameWatcher.OnPlayerDies += SaveCurrentAttempt;
            GameWatcher.OnPlayerWins += SaveCurrentWinningAttempt;
        }

        public void StartRecording()
        {
            GameWatcher.StartWatching();
        }

        public void StopRecording()
        {
            GameWatcher.StopWatching();
        }

        public void SaveCurrentAttempt(GameState state)
        {
            CurrentAttempt.EndPercent = 100 * state.PlayerObject.XPosition / state.PlayedLevel.PhysicalLength;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        public void SaveCurrentWinningAttempt(GameState state)
        {
            CurrentAttempt.EndPercent = 100;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        public void CreateNewSession(GameState state)
        {
            var playedLevel = new Level(state.PlayedLevel);
            CurrentSession = new Session
            {
                Level = playedLevel,
                StartPercent = float.PositiveInfinity,//branle TODO OnLevelSettingsLoaded ?
                StartTime = DateTime.Now,
                Attempts = new List<Attempt>(),
            };
        }

        public void SaveCurrentSession(GDLoadedLevelInfos level)
        {
            if (CurrentSession.Level.PhysicalLength <= Level.LENGTH_EPSILON)//TODO not very clean, but length is not initialized right when the level loads
            {
                CurrentSession.Level.PhysicalLength = level.PhysicalLength;
            }
            SaveCurrentSession();
        }

        public void SaveCurrentSession()
        {
            if (CurrentSession == null) return;
            CurrentSession.IsCopy = CurrentSession.StartPercent <= MIN_PERCENT_COPY;
            SessionSaver.SaveSession(CurrentSession);
        }

        public void UpdateSessionStartPercent(GameState state)
        {
            var percent = 100 * state.PlayerObject.XPosition / state.PlayedLevel.PhysicalLength;
            if (percent < CurrentSession.StartPercent)
            {
                CurrentSession.StartPercent = percent;
            }
        }

        public void CreateNewAttempt(GameState state)
        {
            CurrentAttempt = new Attempt()
            {
                StartTime = DateTime.Now,
                Number = state.PlayedLevel.AttemptNumber,
            };
        }
    }
}
