using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.DataModel.SerializedData;
using Whydoisuck.MemoryReading;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataModel
{
    public class Session : IWDISSerializable
    {
        public Level Level { get; set; }
        public bool IsCopyRun { get; set; }
        public DateTime StartTime { get; set; }
        public float StartPercent { get; set; }
        public List<Attempt> Attempts { get; set; }

        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }

        public string GetDefaultSessionFileName()
        {
            return $"{Level.Name} rev{Level.Revision}";
        }

        public static int CompareStart(Session s, Session s2)
        {
            return (int)((s.StartPercent - s2.StartPercent) / Math.Abs(s.StartPercent - s2.StartPercent));
        }

        public string GetSerializedObject()
        {
            var serializedItem = new SerializedSession(this);
            return serializedItem.Serialize();
        }

        public void InitFromSerialized(string value)
        {
            var serializedSession = new SerializedSession(value);
            Level = new Level(serializedSession.Level);
            IsCopyRun = serializedSession.IsCopyRun;
            StartTime = serializedSession.StartTime;
            StartPercent = serializedSession.StartPercent;
            Attempts = serializedSession.Attempts.Select(a => new Attempt(a)).ToList();
        }
    }
}
