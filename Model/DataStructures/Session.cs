using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;
using Whydoisuck.Utilities;

namespace Whydoisuck.Model.DataStructures
{
    public class Session : IWDISSerializable
    {
        [JsonProperty(PropertyName ="SessionName")] public string SessionName { get; set; }
        [JsonProperty(PropertyName = "Level")] public Level Level { get; set; }
        [JsonProperty(PropertyName = "IsCopyRun")] public bool IsCopyRun { get; set; }
        [JsonProperty(PropertyName = "StartTime")] public DateTime StartTime { get; set; }
        [JsonProperty(PropertyName = "StartPercent")] public float StartPercent { get; set; }
        [JsonProperty(PropertyName = "Attempts")] public List<Attempt> Attempts { get; set; }

        public Session() { } //for json deserializer //TODO make public

        public Session(DateTime startTime)
        {
            Attempts = new List<Attempt>();
            StartTime = startTime;
        }

        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }

        public string GetDefaultSessionFileName()
        {
            return $"{Level.Name}" + (Level.Revision == 0 ? "" : $" rev{Level.Revision}");
        }

        public static int CompareStart(Session s, Session s2)
        {
            return (int)((s.StartPercent - s2.StartPercent) / Math.Abs(s.StartPercent - s2.StartPercent));
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public override void Deserialize(string value)
        {
            JsonConvert.PopulateObject(value, this);
        }
    }
}
