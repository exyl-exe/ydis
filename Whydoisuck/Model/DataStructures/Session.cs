using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ydis.Model.MemoryReading.GameStateStructures;
using Ydis.Model.UserSettings;

namespace Ydis.Model.DataStructures
{
    /// <summary>
    /// Session played in the game. A session starts when entering a level and ends when exiting it.
    /// </summary>
    public class Session : ISession
    {
        /// <summary>
        /// Level the session was played on
        /// </summary>
        [JsonProperty(PropertyName = "Level")] public Level Level { get; set; }
        /// <summary>
        /// True if the session was played from a startpos.
        /// </summary>
        [JsonProperty(PropertyName = "IsCopyRun")] public bool IsCopyRun { get; set; }
        /// <summary>
        /// Time the session was started at
        /// </summary>
        [JsonProperty(PropertyName = "StartTime")] public DateTime StartTime { get; set; }
        /// <summary>
        /// How long was the session
        /// </summary>
        [JsonProperty(PropertyName = "Duration")] public TimeSpan Duration{ get; set; }
        /// <summary>
        /// Percent the attempts in the session were starting at
        /// </summary>
        [JsonProperty(PropertyName = "StartPercent")] public float StartPercent { get; set; }
        /// <summary>
        /// List of attempts played in the session
        /// </summary>
        [JsonProperty(PropertyName = "Attempts")] public List<Attempt> Attempts { get; set; }

        public Session() { } //for deserialization

        public Session(GameState state, DateTime startTime)
        {
            Level = new Level(state);
            StartTime = startTime;
            IsCopyRun = state.LoadedLevel.IsTestmode;
            StartPercent = 100 * state.LoadedLevel.StartPosition / state.LoadedLevel.PhysicalLength;
            Attempts = new List<Attempt>();
        }

        /// <summary>
        /// Adds an attempt to the session
        /// </summary>
        /// <param name="att">The attempt to add</param>
        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }
    }
}
