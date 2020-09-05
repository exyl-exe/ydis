using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;

namespace Whydoisuck.Recording
{
    class Recorder
    {
        private GDMemoryReader Reader { get; set; }
        private int Delay { get; set; } = 100;//ms
        private bool IsRecording { get; set; }
        private int CurrentLevelID { get; set; }
        private Session CurrentSession { get; set; }
        private Attempt CurrentAttempt { get; set; }
        private bool IsCurrentAttemptSaved { get; set; }

        public Recorder()
        {
            Reader = new GDMemoryReader();
        }

        public void RecordThread()
        {
            IsRecording = true;
            while (IsRecording)
            {
                Thread.Sleep(Delay);
                TrySave();
            }
        }

        public Thread StartRecording()
        {
            Reader.Initialize();
            var thread = new Thread(new ThreadStart(RecordThread));
            thread.Start();
            return thread;
        }

        public void StopRecording()
        {
            IsRecording = false;
        }

        public void TrySave()
        {
            if (!Reader.IsInitialized)
            {
                if (CurrentSession != null) SaveCurrentSession();
                Reader.Initialize();
                return;
            }

            Reader.Update();

            //Current level was exited
            if (Reader.Level == null || Reader.Level.ID != CurrentLevelID)
            {
                if(CurrentSession != null)
                {
                    SaveCurrentSession();
                    CurrentSession = null;
                }
                return;
            }

            //A level was entered
            if (Reader.Level != null && CurrentSession == null)
            {
                CurrentLevelID = Reader.Level.ID;
                CreateNewSession();
                CreateNewAttempt();
                IsCurrentAttemptSaved = false;
            }

            //Current attempt has stopped and needs to be saved. If somehow another attempt has already started, give up on this attempt
            //TODO proper way to handle lost attempts
            if (!IsAnAttemptOngoing() && !IsCurrentAttemptSaved && CurrentAttempt.Number == Reader.Level.CurrentAttempt)
            {
                SaveCurrentAttempt();
                IsCurrentAttemptSaved = true;
                return;
            }

            //An attempt just started (excluding first attempt)
            if (CurrentAttempt.Number != Reader.Level.CurrentAttempt)
            {
                CreateNewAttempt();
                IsCurrentAttemptSaved = false;
            }
        }

        public bool IsAnAttemptOngoing()
        {
            return !Reader.Player.IsDead && !Reader.Player.HasWon;
        }

        public void SaveCurrentSession()
        {
            if (CurrentSession == null) return;

            //TODO save to json
            var session = $"session:{CurrentSession.startTime}, {CurrentSession.Attempts.Count} attempts";
            foreach(var att in CurrentSession.Attempts)
            {
                session += $"\n\t Attempt {att.Number}, {att.StartPercent}%-{att.EndPercent}%, {att.Duration.TotalSeconds}s";
            }
            TempLogger.AddLog(session);
        }

        public void SaveCurrentAttempt()
        {
            if(CurrentSession == null) throw new Exception("Saved an attempt without a session being created beforehand");

            CurrentAttempt.StartPercent = Reader.Level.StartPosition;
            CurrentAttempt.EndPercent = 100 * Reader.Player.XPosition / Reader.Level.Length;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
        }

        public void CreateNewSession()
        {
            CurrentSession = new Session
            {
                LevelID = Reader.Level.ID,
                Attempts = new List<Attempt>(),
                startTime = DateTime.Now
            };
        }

        public void CreateNewAttempt()
        {
            CurrentAttempt = new Attempt()
            {
                StartTime = DateTime.Now,
                Number = Reader.Level.CurrentAttempt
            };
        }
    }
}
