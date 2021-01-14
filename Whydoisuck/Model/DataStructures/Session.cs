using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Model.UserSettings;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// Session played in the game. A session starts when entering a level and ends when exiting it.
    /// </summary>
    public class Session : WDISSerializable
    {
        /// <summary>
        /// Name of the session
        /// </summary>
        [JsonProperty(PropertyName ="SessionName")] public string SessionName { get; set; }
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

        public Session() { } //for json deserializer

        public Session(DateTime startTime)
        {
            Attempts = new List<Attempt>();
            StartTime = startTime;
        }

        /// <summary>
        /// Adds an attempt to the session
        /// </summary>
        /// <param name="att">The attempt to add</param>
        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }

        /// <summary>
        /// Gets the default file name for the session
        /// </summary>
        /// <returns>The default file name for this session</returns>
        public string GetDefaultSessionFileName()
        {
            return string.Format("{0}{1:ddMMyyyy}_{2:hhmm}",
                Level.Name + (Level.Revision == 0 ? "" : $"_rev{Level.Revision}"),
                StartTime.Date,
                StartTime.TimeOfDay);
        }
    }
}
