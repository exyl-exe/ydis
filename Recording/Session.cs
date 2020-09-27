using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;
using Whydoisuck.UIModel;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    public class Session
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

        public List<SessionAttempt> GetSessionAttempts()
        {
            return Attempts.Select(a => new SessionAttempt(this, a)).ToList();
        }

        public string GetDefaultSessionFileName()
        {
            return $"{Level.Name} rev{Level.Revision}";
        }

        public void CreateSessionFile(string path)
        {
            var sessionJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            SafeFile.WriteAllText(path, sessionJson);
        }

        public static int CompareStart(Session s, Session s2)
        {
            return (int)((s.StartPercent - s2.StartPercent) / Math.Abs(s.StartPercent - s2.StartPercent));
        }
    }
}
