using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;
using Whydoisuck.Model.DataStructures;
using Whydoisuck.Model.MemoryReading.GameStateStructures;

namespace Whydoisuck.DataSaving
{
    public class Recorder
    {
        private Attempt CurrentAttempt { get; set; }
        public Session CurrentSession { get; set; }
        public SessionManager Manager { get; set; }

        public delegate void SessionCallback(Session session);
        public delegate void AttemptCallback(Attempt attempt);

        public event SessionCallback OnNewCurrentSessionInitialized;
        public event SessionCallback OnQuitCurrentSession;
        public event AttemptCallback OnAttemptAdded;


        public Recorder()
        {
            GameWatcher.OnLevelEntered += CreateNewSession;
            GameWatcher.OnLevelStarted += UpdateCurrentSession;
            GameWatcher.OnLevelExited += PopSaveCurrentSession;
            GameWatcher.OnPlayerSpawns += CreateNewAttempt;
            GameWatcher.OnPlayerDies += PopSaveLosingAttempt;
            GameWatcher.OnPlayerRestarts += PopSaveLosingAttempt;
            GameWatcher.OnPlayerWins += PopSaveWinningAttempt;
        }

        public void StartRecording()
        {
            Manager = SerializationManager.DeserializeSessionManager();
            GameWatcher.StartWatching();
        }

        public void StopRecording()
        {
            GameWatcher.StopWatching();
            SerializationManager.SerializeSessionManager(Manager);
        }

        //Called when entering a level, ensure a session is created before an attempt needs to be saved
        //However, while its metadata is fully loaded, the level is not
        //Therefore stuff like the level length, the start position etc. is updated when the level is fully loaded and not in this function
        private void CreateNewSession(GDLevelMetadata level)
        {
            CurrentSession = new Session(DateTime.Now);
        }

        //Update values for the current session, is called when the level is fully loaded
        private void UpdateCurrentSession(GameState state)
        {
            CreateSessionIfNotExists(state);
            CurrentSession.Level = new Level(state);
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;
            CurrentSession.StartPercent = 100 * state.LoadedLevel.StartPosition / state.LoadedLevel.PhysicalLength;
            OnNewCurrentSessionInitialized?.Invoke(CurrentSession);
        }

        private void PopSaveCurrentSession(GDLevelMetadata level)
        {
            //Don't save if :
            //  -no session were created (= software launched while playing a level, and no attempts have been played before exiting)
            //  -The current level is unknown (= The level was left before it finished loading)
            //  -There are not attempts in the session (= useless data)
            if (CurrentSession == null || CurrentSession.Level == null || CurrentSession.Attempts.Count == 0) return;
            Manager.SaveSession(CurrentSession);
            SerializationManager.SerializeSessionManager(Manager);

            OnQuitCurrentSession?.Invoke(CurrentSession);
            CurrentSession = null;
            CurrentAttempt = null;
        }

        private void CreateNewAttempt(GameState state)
        {
            CurrentAttempt = new Attempt(state.LoadedLevel.AttemptNumber, DateTime.Now);
        }

        private void PopSaveLosingAttempt(GameState state)
        {
            CreateSessionIfNotExists(state);
            CreateAttemptIfNotExists(state);
            CurrentAttempt.EndPercent = 100 * state.PlayerObject.XPosition / state.LoadedLevel.PhysicalLength;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
            OnAttemptAdded?.Invoke(CurrentAttempt);
            CurrentAttempt = null;
        }

        private void PopSaveWinningAttempt(GameState state)
        {
            CreateSessionIfNotExists(state);
            CreateAttemptIfNotExists(state);
            CurrentAttempt.EndPercent = 100;
            CurrentAttempt.Duration = DateTime.Now - CurrentAttempt.StartTime;
            CurrentSession.AddAttempt(CurrentAttempt);
            OnAttemptAdded?.Invoke(CurrentAttempt);
            CurrentAttempt = null;
        }

        //Creates a session if there is no current session and initialize known values
        private void CreateSessionIfNotExists(GameState state)
        {
            if (CurrentSession != null) return;
            CurrentSession = new Session(DateTime.Now);

            if (state == null || state.LevelMetadata == null || state.LoadedLevel == null) return;
            CurrentSession.Level = new Level(state);
            CurrentSession.IsCopyRun = state.LoadedLevel.IsTestmode;
            CurrentSession.StartPercent = 100 * state.LoadedLevel.StartPosition / state.LoadedLevel.PhysicalLength;
            OnNewCurrentSessionInitialized?.Invoke(CurrentSession);
        }

        //Creates an attempt if there is no current attempt and initialize known values
        private void CreateAttemptIfNotExists(GameState state)
        {
            if (CurrentAttempt != null) return;
            CurrentAttempt = new Attempt(state.LoadedLevel.AttemptNumber, DateTime.Now);

            if (state.LoadedLevel == null) return;
            CurrentAttempt.Number = state.LoadedLevel.AttemptNumber;
        }
    }
}
