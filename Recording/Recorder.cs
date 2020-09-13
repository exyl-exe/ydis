﻿using System;
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
            //TODO minimalize the impact of the order in which events are fired
            GameWatcher.OnPlayerObjectCreated += CreateNewSession;
            GameWatcher.OnLevelExited += SaveCurrentSession;
            GameWatcher.OnPlayerSpawns += CreateNewAttempt;
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
            CurrentAttempt.EndPercent = 100 * state.PlayerObject.XPosition / state.PlayedLevel.Length;
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
                StartTime = DateTime.Now,
                Attempts = new List<Attempt>(),
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
            SessionSaver.SaveSession(CurrentSession);
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
