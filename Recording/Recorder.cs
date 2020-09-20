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

        public Recorder()
        {
            GameWatcher.OnLevelEntered += CreateNewSession;
            GameWatcher.OnLevelStarted += UpdateCurrentSession;
            GameWatcher.OnLevelExited += SaveCurrentSession;
            GameWatcher.OnPlayerSpawns += CreateNewAttempt;
            GameWatcher.OnPlayerDies += SaveDeathAttempt;
            GameWatcher.OnPlayerWins += SaveWinAttempt;
        }

        public void StartRecording()
        {
            GameWatcher.StartWatching();
        }

        public void StopRecording()
        {
            GameWatcher.StopWatching();
        }

        public void SaveDeathAttempt(GameState state)
        {
            CurrentAttempt.EndPercent = 100 * state.PlayerObject.XPosition / state.LoadedLevel.PhysicalLength;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        public void SaveWinAttempt(GameState state)
        {
            CurrentAttempt.EndPercent = 100;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        //Called when entering a level, ensure a session is created before an attempt needs to be saved
        //However, while its metadata is fully loaded, the level is not
        //Therefore stuff like the level length, the start position etc. is updated when the level is fully loaded and not in this function
        public void CreateNewSession(GDLevelMetadata level)
        {
            CurrentSession = new Session
            {
                Attempts = new List<Attempt>(),
            };
        }

        //Update values for the current session, is called when the level is fully loaded
        public void UpdateCurrentSession(GameState state)
        {
            TempLogger.AddLog("Updated");
            CurrentSession.Level = new Level(state);
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;
            CurrentSession.StartPercent = 100 * state.LoadedLevel.StartPosition / state.LoadedLevel.PhysicalLength;
            CurrentSession.StartTime = DateTime.Now;
        }

        public void SaveCurrentSession(GDLevelMetadata level)
        {
            SessionSaver.SaveSession(CurrentSession);
        }

        public void CreateNewAttempt(GameState state)
        {
            CurrentAttempt = new Attempt()
            {
                StartTime = DateTime.Now,
                Number = state.LoadedLevel.AttemptNumber,
            };
        }
    }
}
