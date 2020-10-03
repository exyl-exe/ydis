using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.DataModel.SerializedData
{
    class SerializedSession : IWdisSerialized
    {
        [JsonProperty(PropertyName = "Level")] public SerializedLevel Level { get; set; }
        [JsonProperty(PropertyName = "IsCopyRun")] public bool IsCopyRun { get; set; }
        [JsonProperty(PropertyName = "StartTime")] public DateTime StartTime { get; set; }
        [JsonProperty(PropertyName = "StartPercent")] public float StartPercent { get; set; }
        [JsonProperty(PropertyName = "Attempts")] public List<SerializedAttempt> Attempts { get; set; }

        public SerializedSession(string value)
        {
            var jsonObject = JObject.Parse(value);
            //TODO throw exception if deserialization fails
            Update(jsonObject["Version"].ToObject<int>(), jsonObject);
            Level = jsonObject["Level"].ToObject<SerializedLevel>();
            IsCopyRun = jsonObject["IsCopyRun"].ToObject<bool>();
            StartTime = jsonObject["StartTime"].ToObject<DateTime>();
            StartPercent = jsonObject["StartPercent"].ToObject<float>();
            Attempts = jsonObject["Attempts"].ToObject<List<SerializedAttempt>>();
        }

        public SerializedSession(Session s)
        {
            DebugLogger.AddLog(s.Level.ID+"");
            Level = new SerializedLevel(s.Level);
            IsCopyRun = s.IsCopyRun;
            StartTime = s.StartTime;
            StartPercent = s.StartPercent;
            Attempts = s.Attempts.Select(attempt => new SerializedAttempt(attempt)).ToList();
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public JObject Update(int version, JObject session)
        {
            return session;
        }
    }
}
