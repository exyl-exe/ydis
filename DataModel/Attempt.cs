using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel
{
    public class Attempt
    {
        [JsonProperty(PropertyName = "Number")] public int Number { get; set; }
        [JsonProperty(PropertyName = "EndPercent")] public float EndPercent { get; set; }
        [JsonProperty(PropertyName = "StartTime")] public DateTime StartTime { get; set; }
        [JsonProperty(PropertyName = "Duration")]  public TimeSpan Duration { get; set; }

        public int Compare(Attempt attempt)
        {
            if (EndPercent > attempt.EndPercent)
            {
                return 1;
            }
            else if (EndPercent < attempt.EndPercent)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
