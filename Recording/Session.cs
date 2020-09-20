using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;
using Whydoisuck.Utilities;

namespace Whydoisuck.DataSaving
{
    class Session
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

        public void CreateSessionFile(string path)
        {
            var sessionJson = JsonConvert.SerializeObject(this, Formatting.Indented);
            SafeFile.WriteAllText(path, sessionJson);
        }
    }
}
