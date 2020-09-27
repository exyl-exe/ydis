using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataModel
{
    public class Session : IWdisSerializable
    {
        [JsonProperty(PropertyName = "Level")] public Level Level { get; set; }
        [JsonProperty(PropertyName = "IsCopyRun")] public bool IsCopyRun { get; set; }
        [JsonProperty(PropertyName = "StartTime")] public DateTime StartTime { get; set; }
        [JsonProperty(PropertyName = "StartPercent")] public float StartPercent { get; set; }
        [JsonProperty(PropertyName = "Attempts")] public List<Attempt> Attempts { get; set; }

        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }

        public List<SessionAttempt> GetSessionAttempts()
        {
            return Attempts.Select(a => new SessionAttempt(this, a)).ToList();
        }

        public string GetDefaultSessionFileName()
        {
            return $"{Level.Name} rev{Level.Revision}";
        }

        public static int CompareStart(Session s, Session s2)
        {
            return (int)((s.StartPercent - s2.StartPercent) / Math.Abs(s.StartPercent - s2.StartPercent));
        }
    }
}
