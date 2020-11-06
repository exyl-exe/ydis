using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.Utilities;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// Session played in the game. A session starts when entering a level and ends when exiting it.
    /// </summary>
    public class Session : IWDISSerializable
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
            return $"{Level.Name}" + (Level.Revision == 0 ? "" : $" rev{Level.Revision}");
        }

        /// <summary>
        /// Gets the json serialization for this session
        /// </summary>
        /// <returns>a string containing the json object for this session</returns>
        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Deserializes a json string containing a session, and inits current object with its values.
        /// </summary>
        /// <param name="value">String to deserialize.</param>
        public override void Deserialize(string value)
        {
            JsonConvert.PopulateObject(value, this);
        }
    }
}
