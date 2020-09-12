using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whydoisuck.MemoryReading;

namespace Whydoisuck.DataSaving
{
    class Session
    {
        public Level Level { get; set; }
        public DateTime StartTime { get; set; }
        public List<Attempt> Attempts { get; set; }

        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }

        public string GetDefaultSessionFileName()
        {
            return $"{Level.Name}_{Level.Revision}";
        }

        public void CreateSessionFile(IndexerEntry entry)
        {
            var sessionJson = JsonConvert.SerializeObject(this);
            var path = entry.GetSessionPath();
            File.WriteAllText(path, sessionJson);
        }
    }
}
