using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Model.DataStructures
{
    /// <summary>
    /// One played attempt on a level.
    /// </summary>
    public class Attempt 
    {
        /// <summary>
        /// The attempt number
        /// </summary>
        [JsonProperty(PropertyName = "Number")] public int Number { get; set; }
        /// <summary>
        /// The percent the player died at
        /// </summary>
        [JsonProperty(PropertyName = "EndPercent")] public float EndPercent { get; set; }
        /// <summary>
        /// The starting time of the attempt
        /// </summary>
        [JsonProperty(PropertyName = "StartTime")] public DateTime StartTime { get; set; } // TODO useless
        /// <summary>
        /// The duration of the attempt
        /// </summary>
        [JsonProperty(PropertyName = "Duration")] public TimeSpan Duration { get; set; } // TODO useless

        public Attempt() { }

        public Attempt(int number, DateTime startTime)
        {
            Number = number;
            StartTime = startTime;
        }
    }
}
