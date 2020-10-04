using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel.SerializedData
{
    public class SerializedAttempt
    {
        [JsonProperty(PropertyName = "Number")] public int Number { get; set; }
        [JsonProperty(PropertyName = "EndPercent")] public float EndPercent { get; set; }
        [JsonProperty(PropertyName = "StartTime")] public DateTime StartTime { get; set; }
        [JsonProperty(PropertyName = "Duration")] public TimeSpan Duration { get; set; }

        public SerializedAttempt() { }

        public SerializedAttempt(Attempt attempt)
        {
            Number = attempt.Number;
            EndPercent = attempt.EndPercent;
            StartTime = attempt.StartTime;
            Duration = attempt.Duration;
        }
    }
}
