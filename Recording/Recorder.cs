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
            //TODO Associate events
        }

        public void StartRecording()
        {
            gameWatcher.StartWatching();
        }

        public void StopRecording()
        {
            gameWatcher.StopWatching();
        }

        public void SaveCurrentSession(GDLoadedLevelInfos level)
        {
            if (CurrentSession == null) return;

            //TODO save to json
            var session = $"[{CurrentSession.LevelName}] {CurrentSession.startTime}, {CurrentSession.Attempts.Count} attempts";
            foreach(var att in CurrentSession.Attempts)
            {
                session += $"\n\t Attempt {att.Number}, {att.StartPercent}%-{att.EndPercent}%, {att.Duration.TotalSeconds}s";
            }
            TempLogger.AddLog(session);
        }

        public void SaveCurrentAttempt()
        {
            if(CurrentSession == null) throw new Exception("Saved an attempt without a session being created beforehand");
            CurrentAttempt.EndPercent = 100 * Reader.Player.XPosition / Reader.Level.Length;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        public void CreateNewSession(GDLoadedLevelInfos level)
        {
            CurrentSession = new Session
            {
                LevelID = Reader.Level.ID,
                Attempts = new List<Attempt>(),
                startTime = DateTime.Now,
                LevelName = Reader.Level.Name
            };
        }

        public void CreateNewAttempt(GDLoadedLevelInfos level)
        {
            CurrentAttempt = new Attempt()
            {
                StartTime = DateTime.Now,
                Number = Reader.Level.CurrentAttempt,
                StartPercent = 100 * Reader.Player.XPosition / Reader.Level.Length//Couldn't find a better way unfortunately
            };
        }
    }
}
