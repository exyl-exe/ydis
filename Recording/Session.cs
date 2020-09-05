using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.Recording
{
    class Session
    {
        public DateTime startTime { get; set; }
        public List<Attempt> Attempts { get; set; }
        public int LevelID { get; set; }
        public string LevelName { get; set; }

        public void AddAttempt(Attempt att)
        {
            Attempts.Add(att);
        }
    }
}
