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
        private Session CurrentSession { get; set; }
        private int CurrentAttemptNumber { get; set; }//TODO replace by Attempt class
        private DateTime CurrentAttemptStartTime { get; set; }
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
                //Reader.Initialize();
                return;
            }

            Reader.Update();

            if (Reader.Level == null)//Current level was exited
            //TODO or level != previousLevel
            {
                if(CurrentSession != null)
                {
                    SaveCurrentSession();
                    CurrentSession = null;
                }
                return;
            }

            if(Reader.Level != null && CurrentSession == null)//A level was entered
            {
                CreateNewSession();
                CurrentAttemptStartTime = DateTime.Now;
                IsCurrentAttemptSaved = false;
            }

            if (!IsAnAttemptOngoing() && !IsCurrentAttemptSaved)//Current attempt has stopped and needs to be saved
            //TODO && CurrentAttempt == Reader.Level.CurrentAttempt
            {
                IsCurrentAttemptSaved = true;
                SaveCurrentAttempt();
                return;
            }

            if(CurrentAttemptNumber != Reader.Level.CurrentAttempt)//An attempt just started (excluding first attempt)
            {
                CurrentAttemptNumber = Reader.Level.CurrentAttempt;
                CurrentAttemptStartTime = DateTime.Now;
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

            var att = new Attempt//TODO actual constructor ?
            {
                StartPercent = 0,//TODO : Reader.Level.StartPosition,
                EndPercent = 100 * Reader.Player.XPosition / Reader.Level.Length,
                Number = CurrentAttemptNumber,
                StartTime = CurrentAttemptStartTime,
                Duration = DateTime.Now - CurrentAttemptStartTime
            };
            CurrentSession.AddAttempt(att);
        }

        public void CreateNewSession()
        {
            CurrentSession = new Session//TODO actual constructor ?
            {
                LevelID = 0,//TODO
                Attempts = new List<Attempt>(),
                startTime = DateTime.Now
            };
        }
    }
}
